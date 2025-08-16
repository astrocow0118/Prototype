namespace Prototype.Gateway.Services;

public class GuestAuthService
{
    public Task<string> Authenticate(string token)
    {
        // TODO: Implement guest authentication logic
        var guestId = Guid.NewGuid().ToString();
        return Task.FromResult($"guest:{guestId}");
    }
}
