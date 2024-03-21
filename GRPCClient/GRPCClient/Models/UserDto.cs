using System.Text.Json.Serialization;

namespace GRPCClient.Models;

public sealed class UserDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("age")]
    public int Age { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("address")]
    public string Address { get; set; }
}

public sealed class UserListResponse
{
    [JsonPropertyName("userList")]
    public List<UserDto> UserList { get; set; }
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }
    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }
    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }
}