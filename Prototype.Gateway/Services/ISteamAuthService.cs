using System.Threading.Tasks;

namespace Prototype.Gateway.Services;

public interface ISteamAuthService
{
    Task<string> Authenticate(string token);
}
