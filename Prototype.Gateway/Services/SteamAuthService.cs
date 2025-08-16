namespace Prototype.Gateway.Services;

public class SteamAuthService : ISteamAuthService
{
    private readonly SteamApiService _steamApiService;

    public SteamAuthService(SteamApiService steamApiService)
    {
        _steamApiService = steamApiService;
    }

    public async Task<string> Authenticate(string token)
    {
        var steamId = await _steamApiService.AuthenticateUserTicket(token);
        if (string.IsNullOrEmpty(steamId))
        {
            throw new Exception("Steam authentication failed.");
        }
        return $"steam:{steamId}";
    }
}
