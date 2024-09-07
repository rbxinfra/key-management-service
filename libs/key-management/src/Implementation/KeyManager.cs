namespace Roblox.KeyManagement;

using System;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;

using VaultSharp;
using VaultSharp.V1.SecretsEngines.Transit;

using Caching;
using EventLog;
using Threading.Extensions;

/// <summary>
/// Implementation of <see cref="IKeyManager"/>.
/// </summary>
/// <seealso cref="IKeyManager"/>
public class KeyManager : IKeyManager
{
    private class PubPrivateKeys
    {
        public Dictionary<string, string> PublicKeys { get; set; } = new();
        public Dictionary<string, string> PrivateKeys { get; set; } = new();
    }

    private const TransitKeyCategory _PrivateKeyType = TransitKeyCategory.signing_key;
    private const string _ExportedKeyDictKey = "1";
    private const string _PublicKeyDictKey = "public_key";

    private readonly ILogger _logger;
    private readonly ITransitSecretsEngine _transit;
    private readonly IKeyManagementSettings _settings;

    private readonly LazyWithRetry<RefreshAhead<PubPrivateKeys>> _keys;

    /// <summary>
    /// Construct a new instance of <see cref="KeyManager"/>.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to use.</param>
    /// <param name="client">The <see cref="IVaultClient"/> to use.</param>
    /// <param name="settings">The <see cref="IKeyManagementSettings"/> to use.</param>
    /// <exception cref="ArgumentNullException">
    /// - <paramref name="logger"/> is null.
    /// - <paramref name="client"/> is null.
    /// - <paramref name="settings"/> is null.
    /// </exception>
    public KeyManager(ILogger logger, IVaultClient client, IKeyManagementSettings settings)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        ArgumentNullException.ThrowIfNull(client, nameof(client));

        _transit = client.V1.Secrets.Transit;

        _keys = new(() => RefreshAhead<PubPrivateKeys>.ConstructAndPopulate(_settings.KeyRefreshInterval, DoKeysRefresh));
    }

    private PubPrivateKeys DoKeysRefresh(PubPrivateKeys current)
    {
        _logger.Information("Refreshing keys.");

        current ??= new();

        var keyNames = _transit.ReadAllEncryptionKeysAsync(mountPoint: _settings.TransitEngineMountPath).SyncOrDefault()?.Data?.Keys;
        if (keyNames == null || !keyNames.Any())
        {
            _logger.Warning("No keys found in Vault.");

            return current;
        }

        var publicKeys = current.PublicKeys;
        var privateKeys = current.PrivateKeys;

        foreach (var keyName in keyNames)
        {
            var keyInfo = _transit.ReadEncryptionKeyAsync(keyName, mountPoint: _settings.TransitEngineMountPath).SyncOrDefault()?.Data;
            if (keyInfo == null)
            {
                _logger.Warning("Failed to read key {0} from Vault.", keyName);
                continue;
            }

            // keyInfo only contains the public key, private key needs to be fetched separately.
            if (!keyInfo.Keys.TryGetValue(_ExportedKeyDictKey, out var exportedKey) || exportedKey is not JsonElement element)
            {
                _logger.Warning("Failed to read public key for {0} from Vault.", keyName);

                continue;
            }

            if (!element.TryGetProperty(_PublicKeyDictKey, out var pubKeyElement))
            {
                _logger.Warning("Failed to read public key for {0} from Vault.", keyName);

                continue;
            }

            publicKeys[keyName] = pubKeyElement.GetString();

            var privateKeyInfo = _transit.ExportKeyAsync(_PrivateKeyType, keyName, mountPoint: _settings.TransitEngineMountPath).SyncOrDefault()?.Data;
            if (privateKeyInfo == null)
            {
                _logger.Warning("Failed to read private key for {0} from Vault.", keyName);

                continue;
            }

            if (!privateKeyInfo.Keys.TryGetValue(_ExportedKeyDictKey, out var exportedPrivateKey) || exportedPrivateKey is not JsonElement privKeyElement)
            {
                _logger.Warning("Failed to read private key for {0} from Vault.", keyName);

                continue;
            }

            privateKeys[keyName] = privKeyElement.GetString();
        }

        current.PublicKeys = publicKeys;
        current.PrivateKeys = privateKeys;

        return current;
    }

    /// <inheritdoc cref="IKeyManager.GetNamedKey(string, KeyType)"/>
    public string GetNamedKey(string keyName, KeyType keyType = KeyType.Public)
    {
        ArgumentNullException.ThrowIfNull(keyName, nameof(keyName));

        return GetAllKeys(keyType).GetValueOrDefault(keyName);
    }

    /// <inheritdoc cref="IKeyManager.GetAllKeys(KeyType)"/>
    public Dictionary<string, string> GetAllKeys(KeyType keyType = KeyType.Public)
    {
        var keys = _keys.Value.Value;
        return keyType switch
        {
            KeyType.Public => new(keys.PublicKeys),
            KeyType.Private => new(keys.PrivateKeys),
            _ => throw new ArgumentOutOfRangeException(nameof(keyType))
        };
    }
}
