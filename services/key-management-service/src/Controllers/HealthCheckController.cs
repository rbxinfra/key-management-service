namespace Roblox.KeyManagement.Service.Controllers;

using System;
using System.Reflection;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Configuration;
using ApplicationContext;

/// <summary>
/// Health check controller.
/// </summary>
[Route("")]
[ApiController]
[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]
public class HealthCheckController : Controller
{
    private readonly IApplicationContext _ApplicationContext;

    /// <summary>
    /// Constructs a new instance of <see cref="HealthCheckController"/>
    /// </summary>
    /// <param name="applicationContext">The <see cref="IApplicationContext"/></param>
    /// <exception cref="ArgumentNullException"><paramref name="applicationContext"/> cannot be null.</exception>
    public HealthCheckController(IApplicationContext applicationContext)
    {
        _ApplicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
    }

    private string GetVersion()
        => GetType().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

    /// <summary>
    /// Check health endpoint.
    /// </summary>
    /// <returns>The health check result.</returns>
    [Route("")]
    [Route("/checkhealth")]
    public ActionResult CheckHealth()
        => Content("Key Management Service is running. " +
                    $"API Control Plane service name: {_ApplicationContext.Name}, " +
                    $"Version: {GetVersion()}, Environment: {EnvironmentNameProvider.EnvironmentName}");
}
