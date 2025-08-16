using System.Text.Json;

namespace Prototype.Gateway.Services;

public class SteamApiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    // It's recommended to store the API key in a secure way, for example, using dotnet user-secrets
    private readonly string? _apiKey;

    public SteamApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _apiKey = _configuration["SteamApiKey"];
    }

    public async Task<string?> AuthenticateUserTicket(string ticket)
    {
        const ulong appId = 480; // 480 is the default for Spacewar, replace with your appid

        var requestUri = $"https://api.steampowered.com/ISteamUserAuth/AuthenticateUserTicket/v1/?key={_apiKey}&appid={appId}&ticket={ticket}";

        try
        {
            var response = await _httpClient.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
            {
                // Log error
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse?.Response.Params?.Result == "OK")
            {
                return apiResponse.Response.Params.SteamId;
            }

            return null;
        }
        catch (Exception)
        {
            // Log exception
            return null;
        }
    }
}

// Records for deserializing the Steam API response
public record ApiResponse(ResponseData Response);
public record ResponseData(ResponseParams Params);
public record ResponseParams(string Result, string SteamId);