namespace Roblox.KeyManagement.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Request data for the <see cref="GetNamedKeyOperation"/>.
/// </summary>
public class GetNamedKeyRequest
{
    /// <summary>
    /// The name of the key to get.
    /// </summary>
    [Required]
    [FromQuery(Name = "keyName")]
    public string KeyName { get; set; }

    /// <summary>
    /// The <see cref="KeyType"/> of the key to get.
    /// </summary>
    [DefaultValue(KeyType.Public)]
    [FromQuery(Name = "keyType")]
    public KeyType KeyType { get; set; } = KeyType.Public;
}