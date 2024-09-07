namespace Roblox.KeyManagement;

using System;

/// <summary>
/// Settings for the key management service.
/// </summary>
public interface IKeyManagementSettings
{
    // TODO: Should RCC have its own keys for client signatures? Only game join tickets are used by RCC,
    //       whereas client signatures are used by the client (they don't verify game join tickets).

    /// <summary>
    /// Gets the transit engine mount path for the key management service.
    /// </summary>
    string TransitEngineMountPath { get; }

    /// <summary>
    /// Gets the refresh interval for the key management service.
    /// </summary>
    TimeSpan KeyRefreshInterval { get; }

    // Key names defined below used for the GetClientPublicKeys operation.
    // Use GetPublicKeys operation to get all public keys.
    // Use GetNamedKey operation to get a specific key.
    // Use GetPrivateKeys operation to get all private keys.

    /// <summary>
    /// Gets the name of the key used for the client signature v1 (rbxsig2).
    /// </summary>
    string ClientSignatureKeyName { get; } // Embedded into RCC and the client.

    /// <summary>
    /// Gets the name of the key used for the client signature v2 (rbxsig4).
    /// </summary>
    string ClientSignatureV2KeyName { get; } // Embedded into RCC and the client.

    /// <summary>
    /// Gets the name of the key used for the game join ticket signature.
    /// </summary>
    string GameJoinTicketSignatureKeyName { get; } // Embedded into RCC and the client (but it's not used by the client).

    /*
        Public key structure (within memory), kPublicKeys:
        static LPCSTR kPublicKeys[] = {
            "", "", // First 2 are empty. (most likely contained legacy keys for rbxsig1 and rbxsig3)
            "-----BEGIN PUBLIC KEY-----\n" ... // Client signature v1.
            "-----BEGIN PUBLIC KEY-----\n" ... // Game join ticket.
            "-----BEGIN PUBLIC KEY-----\n" ... // Client signature v2.
        };
    */
}