using System.Threading.Tasks;

namespace Prototype.Gateway.Services;

public interface IGuestAuthService
{
    Task<string> Authenticate(string token);
}
