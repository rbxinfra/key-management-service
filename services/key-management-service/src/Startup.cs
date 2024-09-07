namespace Roblox.KeyManagement.Service;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using Roblox.Web.Framework.Services;
using Roblox.Web.Framework.Services.Http;

using Roblox.Vault;

/// <summary>
/// Startup class for ephemeral-counters-service.
/// </summary>
public class Startup : HttpStartupBase
{
    /// <inheritdoc cref="StartupBase.Settings"/>
    protected override IServiceSettings Settings => KeyManagement.Service.Settings.Singleton;

    /// <inheritdoc cref="StartupBase.ConfigureServices(IServiceCollection)"/>
    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
            options.SuppressConsumesConstraintForFormFileParameters = true;
        });

        var vaultClient = VaultClientFactory.Singleton.GetClient(
            KeyManagement.Service.Settings.Singleton.KeyManagementVaultAddress,
            KeyManagement.Service.Settings.Singleton.KeyManagementVaultToken
        );

        services.AddSingleton(vaultClient);
        services.AddSingleton<IKeyManagementSettings>(KeyManagement.Service.Settings.Singleton);

        services.AddSingleton<IKeyManager, KeyManager>();

        services.AddSingleton<IKeyManagementOperations, KeyManagementOperations>();
    }
}