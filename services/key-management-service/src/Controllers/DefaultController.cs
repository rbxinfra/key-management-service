namespace Roblox.KeyManagement.Service.Controllers;

using Microsoft.AspNetCore.Mvc;

using Roblox.Web.Framework.Services.Http;

using Models;
using Microsoft.AspNetCore.Authorization;

/// <summary>
/// Default controller.
/// </summary>
[Route("")]
[ApiController]
public class DefaultController : Controller
{
    private readonly IOperationExecutor _OperationExecutor;
    private readonly IKeyManagementOperations _KeyManagementOperations;

    /// <summary>
    /// Construct a new instance of <see cref="DefaultController"/>
    /// </summary>
    /// <param name="operationExecutor">The <see cref="IOperationExecutor"/></param>
    /// <param name="apiControlPlaneOperations">The <see cref="IKeyManagementOperations"/></param>
    /// <exception cref="ArgumentNullException">
    /// - <paramref name="operationExecutor"/> cannot be null.
    /// - <paramref name="apiControlPlaneOperations"/> cannot be null.
    /// </exception>
    public DefaultController(IOperationExecutor operationExecutor, IKeyManagementOperations apiControlPlaneOperations)
    {
        _OperationExecutor = operationExecutor ?? throw new ArgumentNullException(nameof(operationExecutor));
        _KeyManagementOperations = apiControlPlaneOperations ?? throw new ArgumentNullException(nameof(apiControlPlaneOperations));
    }

    /// <summary>
    /// Gets the public keys used by RCC service and the client.
    /// </summary>
    /// <returns>The public keys</returns>
    /// <response code="200">The public keys</response>
    /// <response code="500">An error occurred</response>
    /// <response code="503">Service unavailable</response>
    [HttpGet, AllowAnonymous]
    [Route($"/v1/{nameof(GetClientPublicKeys)}")]
    [ProducesResponseType(200, Type = typeof(ClientPublicKeysPayload))]
    [ProducesResponseType(500)]
    [ProducesResponseType(503)]
    public IActionResult GetClientPublicKeys()
        => _OperationExecutor.Execute(_KeyManagementOperations.GetClientPublicKeys);

    /// <summary>
    /// Gets the named key.
    /// </summary>
    /// <param name="request">The <see cref="GetNamedKeyRequest"/></param>
    /// <returns>The named key</returns>
    /// <response code="200">The named key</response>
    /// <response code="400">The key name cannot be null or empty</response>
    /// <response code="500">An error occurred</response>
    /// <response code="503">Service unavailable</response>
    [HttpGet]
    [Route($"/v1/{nameof(GetNamedKey)}")]
    [ProducesResponseType(200, Type = typeof(StringPayload))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    [ProducesResponseType(503)]
    public IActionResult GetNamedKey(GetNamedKeyRequest request)
        => _OperationExecutor.Execute(_KeyManagementOperations.GetNamedKey, request);
}
