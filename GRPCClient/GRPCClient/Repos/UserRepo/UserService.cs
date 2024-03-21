using GRPCClient.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace GRPCClient.Repos.UserRepo;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;

    public UserService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<int> CreateUserAsync(CreateUserDto createUserRequest)
    {
        string? createUserJson = JsonSerializer.Serialize(createUserRequest);

        StringContent content = new StringContent(createUserJson, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"https://localhost:7175/v1/users/", content);

        response.EnsureSuccessStatusCode();

        CreateUserResponseDto createUserResponse = await response.Content.ReadFromJsonAsync<CreateUserResponseDto>();

        return createUserResponse.Id;
    }
    public async Task<UserDto> GetUserAsync(int id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7175/v1/users/{id}");

        if (!response.IsSuccessStatusCode)
            throw new ApplicationException($"Failed to send message. Status code: {response.StatusCode}");

        UserDto result = JsonSerializer.Deserialize<UserDto>(await response.Content.ReadAsStringAsync());

        return JsonSerializer.Deserialize<UserDto>(await response.Content.ReadAsStringAsync());
    }
    public async Task<UserListResponse> GetUsersListAsync(int pageSize = 10, int pageNumber = 1)
    {
        var authenticationToken = await GetAuthenticationTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationToken.AccessToken);

        HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7175/v1/users?page_number={pageNumber}&page_size={pageSize}");

        if (!response.IsSuccessStatusCode)
            throw new ApplicationException($"Failed to send message. Status code: {response.StatusCode}");

        UserListResponse result = JsonSerializer.Deserialize<UserListResponse>(await response.Content.ReadAsStringAsync());

        return JsonSerializer.Deserialize<UserListResponse>(await response.Content.ReadAsStringAsync());
    }
    public async Task<DeleteUserResponse> DeleteUserAsync(int id)
    {
        HttpResponseMessage? response = await _httpClient.DeleteAsync($"https://localhost:7175/v1/users/{id}");

        if (!response.IsSuccessStatusCode)
            throw new ApplicationException($"Failed to send message. Status code: {response.StatusCode}");

        return JsonSerializer.Deserialize<DeleteUserResponse>(await response.Content.ReadAsStringAsync());
    }

    private async Task<AuthenticationResponseDto> GetAuthenticationTokenAsync()
    {
        string? authenticationTokenRequestJson = JsonSerializer.Serialize(new AuthenticationRequestDto()
        {
             UserName = "muhammadjamal",
             Password = "admin@123"
        });

        StringContent content = new StringContent(authenticationTokenRequestJson, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"https://localhost:7175/authenticateuser", content);

        response.EnsureSuccessStatusCode();

        AuthenticationResponseDto authenticationResponse = await response.Content.ReadFromJsonAsync<AuthenticationResponseDto>();

        return authenticationResponse;
    }
}
