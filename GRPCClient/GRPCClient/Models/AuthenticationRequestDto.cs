using System.Text.Json.Serialization;

namespace GRPCClient.Models;

public sealed class AuthenticationRequestDto
{
    [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }
}
public sealed class AuthenticationResponseDto
{
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; }

    [JsonPropertyName("expiresIn")]
    public int  ExpiresIn { get; set; }
}