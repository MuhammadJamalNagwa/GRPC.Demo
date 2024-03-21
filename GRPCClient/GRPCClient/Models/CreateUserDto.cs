using System.Text.Json.Serialization;

namespace GRPCClient.Models;

public sealed class CreateUserDto
{
    [JsonPropertyName("age")]
    public int Age { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("address")]
    public string Address { get; set; }
}
