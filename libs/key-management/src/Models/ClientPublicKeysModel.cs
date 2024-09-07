namespace Roblox.KeyManagement.Models;

using Newtonsoft.Json;

/// <summary>
/// Represents the public keys used by the client and RCC.
/// </summary>
public class ClientPublicKeysModel
{
    /// <summary>
    /// Gets or sets the client signature v2 public key.
    /// </summary>
    /// <remarks>rbxsig2, index 2 of kPublicKeys.</remarks>
    [JsonProperty("rbxsig2")]
    public string ClientSignature { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the client signature v4 public key.
    /// </summary>
    /// <remarks>rbxsig4, index 4 of kPublicKeys.</remarks>
    [JsonProperty("rbxsig4")]
    public string ClientSignatureV2 { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the game join ticket public key.
    /// </summary>
    /// <remarks>index 3 of kPublicKeys.</remarks>
    [JsonProperty("gameJoin")]
    public string GameJoinTicket { get; set; } = string.Empty;
}
