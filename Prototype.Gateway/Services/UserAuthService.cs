using Orleans;
using Prototype.Contracts;

namespace Prototype.Gateway.Services;

public class UserAuthService
{
    private readonly IClusterClient _clusterClient;
    private readonly GuestAuthService _guestAuthService;
    private readonly SteamAuthService _steamAuthService;

    public UserAuthService(IClusterClient clusterClient, GuestAuthService guestAuthService, SteamAuthService steamAuthService)
    {
        _clusterClient = clusterClient;
        _guestAuthService = guestAuthService;
        _steamAuthService = steamAuthService;
    }

    public async Task<Account> AuthenticateAsync(AuthProvider provider, string token)
    {
        var accountKey = provider switch
        {
            AuthProvider.Guest => await _guestAuthService.Authenticate(token),
            AuthProvider.Steam => await _steamAuthService.Authenticate(token),
            _ => throw new NotSupportedException($"Provider '{provider}' is not supported.")
        };

        var accountGrain = _clusterClient.GetGrain<IAccountGrain>(accountKey);
        return await accountGrain.Authenticate();
    }
}
