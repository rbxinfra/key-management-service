namespace Roblox.KeyManagement;

using System;

using EventLog;
using Operations;

using Models;

/// <summary>
/// Operation for getting the public keys of a client.
/// </summary>
/// <remarks>Anonymous access is allowed to this operation so that patcher doesn't need an API key embedded into it.</remarks>
public class GetClientPublicKeysOperation : IResultOperation<ClientPublicKeysModel>
{
    private readonly ILogger _logger;
    private readonly IKeyManager _keyManager;
    private readonly IKeyManagementSettings _settings;

    /// <summary>
    /// Contruct a new instance of <see cref="GetClientPublicKeysOperation"/>.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> to use.</param>
    /// <param name="keyManager">The <see cref="IKeyManager"/> to use.</param>
    /// <param name="settings">The <see cref="IKeyManagementSettings"/> to use.</param>
    /// <exception cref="ArgumentNullException">
    /// - <paramref name="logger"/> is null.
    /// - <paramref name="keyManager"/> is null.
    /// - <paramref name="settings"/> is null.
    /// </exception>
    public GetClientPublicKeysOperation(ILogger logger, IKeyManager keyManager, IKeyManagementSettings settings)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _keyManager = keyManager ?? throw new ArgumentNullException(nameof(keyManager));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    /// <inheritdoc cref="IResultOperation{T}.Execute"/>
    public (ClientPublicKeysModel Output, OperationError Error) Execute()
    {
        var model = new ClientPublicKeysModel();

        var clientSignatureV2Key = _keyManager.GetNamedKey(_settings.ClientSignatureKeyName);
        if (clientSignatureV2Key == null)
            _logger.Warning("Client signature V2 key not found.");

        var gameJoinTicketKey = _keyManager.GetNamedKey(_settings.GameJoinTicketSignatureKeyName);
        if (gameJoinTicketKey == null)
            _logger.Warning("Game join ticket key not found.");

        var clientSignatureV4Key = _keyManager.GetNamedKey(_settings.ClientSignatureV2KeyName);
        if (clientSignatureV4Key == null)
            _logger.Warning("Client signature V4 key not found.");

        model.ClientSignature = clientSignatureV2Key;
        model.GameJoinTicket = gameJoinTicketKey;
        model.ClientSignatureV2 = clientSignatureV4Key;

        return (model, null);
    }
}