namespace Roblox.KeyManagement;

using System.Collections.Generic;

/// <summary>
/// The key manager interface.
/// </summary>
public interface IKeyManager
{
    /// <summary>
    /// Gets a named key from the key manager.
    /// </summary>
    /// <param name="keyName">The name of the key to get.</param>
    /// <param name="keyType">The <see cref="KeyType"/> of the key to get.</param>
    /// <returns>The key as a plain text PEM string, or null if the key does not exist.</returns>
    string GetNamedKey(string keyName, KeyType keyType = KeyType.Public);

    /// <summary>
    /// Gets all keys from the key manager.
    /// </summary>
    /// <param name="keyType">The <see cref="KeyType"/> of the keys to get.</param>
    /// <returns>A dictionary of key names to key PEM strings.</returns>
    Dictionary<string, string> GetAllKeys(KeyType keyType = KeyType.Public);
}
