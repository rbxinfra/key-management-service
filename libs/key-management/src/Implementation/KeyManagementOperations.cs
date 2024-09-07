namespace Roblox.KeyManagement;

using System;

using EventLog;

/// <summary>
/// Implementation of <see cref="IKeyManagementOperations"/>.
/// </summary>
/// <seealso cref="IKeyManagementOperations"/>
public class KeyManagementOperations : IKeyManagementOperations
{
    /// <inheritdoc cref="IKeyManagementOperations.GetClientPublicKeys"/>
    public GetClientPublicKeysOperation GetClientPublicKeys { get; }

    /// <inheritdoc cref="IKeyManagementOperations.GetNamedKey"/>
    public GetNamedKeyOperation GetNamedKey { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="KeyManagementOperations"/>.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to use.</param>
    /// <param name="keyManager">The <see cref="IKeyManager"/> to use.</param>
    /// <param name="settings">The <see cref="IKeyManagementSettings"/> to use.</param>
    /// <exception cref="ArgumentNullException">
    /// - <paramref name="logger"/> is null.
    /// - <paramref name="keyManager"/> is null.
    /// - <paramref name="settings"/> is null.
    /// </exception>
    public KeyManagementOperations(ILogger logger, IKeyManager keyManager, IKeyManagementSettings settings)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(keyManager, nameof(keyManager));
        ArgumentNullException.ThrowIfNull(settings, nameof(settings));

        GetClientPublicKeys = new(logger, keyManager, settings);
        GetNamedKey = new(keyManager);
    }
}