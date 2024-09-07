namespace Roblox.KeyManagement;

/// <summary>
/// Operations for key management.
/// </summary>
public interface IKeyManagementOperations
{
    /// <summary>
    /// Gets the <see cref="GetClientPublicKeysOperation"/>.
    /// </summary>
    GetClientPublicKeysOperation GetClientPublicKeys { get; }

    /// <summary>
    /// Gets the <see cref="GetNamedKeyOperation"/>.
    /// </summary>
    GetNamedKeyOperation GetNamedKey { get; }
}