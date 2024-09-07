namespace Roblox.KeyManagement.Service;

using EventLog;
using Configuration;
using Web.Framework.Services;

using static SettingsProvidersDefaults;

internal class Settings : BaseSettingsProvider<Settings>, IServiceSettings, IKeyManagementSettings
{
    /// <inheritdoc cref="IVaultProvider.Path"/>
    protected override string ChildPath => KeyManagementSettingsPath;

    /// <inheritdoc cref="IServiceSettings.ApiKey"/>
    public string ApiKey => GetOrDefault(nameof(ApiKey), string.Empty);

    /// <inheritdoc cref="IServiceSettings.LogLevel"/>
    public LogLevel LogLevel => GetOrDefault(nameof(LogLevel), LogLevel.Information);

    /// <summary>
    /// Gets the address for the Vault server used for key management.
    /// </summary>
    public string KeyManagementVaultAddress => GetOrDefault<string>(nameof(KeyManagementVaultAddress), () => throw new InvalidOperationException("KeyManagementVaultAddress is not set."));

    /// <summary>
    /// Gets the token for the Vault server used for key management.
    /// </summary>
    public string KeyManagementVaultToken => GetOrDefault<string>(nameof(KeyManagementVaultToken), () => throw new InvalidOperationException("KeyManagementVaultToken is not set."));

    /// <inheritdoc cref="IKeyManagementSettings.KeyRefreshInterval"/>
    public TimeSpan KeyRefreshInterval => GetOrDefault(nameof(KeyRefreshInterval), TimeSpan.FromMinutes(5));

    /// <inheritdoc cref="IKeyManagementSettings.TransitEngineMountPath"/>
    public string TransitEngineMountPath => GetOrDefault(nameof(TransitEngineMountPath), "rbx-kms");

    /// <inheritdoc cref="IKeyManagementSettings.ClientSignatureKeyName"/>
    public string ClientSignatureKeyName => GetOrDefault(nameof(ClientSignatureKeyName), "ClientSignatureKey");

    /// <inheritdoc cref="IKeyManagementSettings.ClientSignatureV2KeyName"/>
    public string ClientSignatureV2KeyName => GetOrDefault(nameof(ClientSignatureV2KeyName), "ClientSignatureKeyV2");

    /// <inheritdoc cref="IKeyManagementSettings.GameJoinTicketSignatureKeyName"/>
    public string GameJoinTicketSignatureKeyName => GetOrDefault(nameof(GameJoinTicketSignatureKeyName), "GameJoinTicketSignature");
}
