namespace Roblox.KeyManagement;

using System.ComponentModel;

/// <summary>
/// Error codes for key management.
/// </summary>
internal enum KeyManagementErrors
{
    /// <summary>
    /// The specified parameter was null or empty.
    /// </summary>
    [Description("{0} cannot be null or empty.")]
    NullOrEmptyParameter,
}