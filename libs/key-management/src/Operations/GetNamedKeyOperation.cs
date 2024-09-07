namespace Roblox.KeyManagement;

using System;

using EventLog;
using Operations;

using Models;

/// <summary>
/// Operation for getting a named key.
/// </summary>
public class GetNamedKeyOperation : IResultOperation<GetNamedKeyRequest, string>
{
    private readonly IKeyManager _keyManager;

    /// <summary>
    /// Contruct a new instance of <see cref="GetNamedKeyOperation"/>.
    /// </summary>
    /// <param name="keyManager">The <see cref="IKeyManager"/> to use.</param>
    /// <exception cref="ArgumentNullException">
    /// - <paramref name="keyManager"/> is null.
    /// </exception>
    public GetNamedKeyOperation(IKeyManager keyManager)
    {
        _keyManager = keyManager ?? throw new ArgumentNullException(nameof(keyManager));
    }

    /// <inheritdoc cref="IResultOperation{TInput, TOutput}.Execute(TInput)"/>
    public (string Output, OperationError Error) Execute(GetNamedKeyRequest input)
    {
        if (string.IsNullOrEmpty(input.KeyName)) return (null, new(KeyManagementErrors.NullOrEmptyParameter, nameof(input.KeyName)));

        return (_keyManager.GetNamedKey(input.KeyName, input.KeyType), null);
    }
}