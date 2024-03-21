using System.Text.Json.Serialization;

namespace GRPCClient.Models;

public sealed class DeleteUserResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
}
