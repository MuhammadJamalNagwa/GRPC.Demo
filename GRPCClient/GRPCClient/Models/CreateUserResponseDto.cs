using System.Text.Json.Serialization;

namespace GRPCClient.Models;

public sealed class CreateUserResponseDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
}