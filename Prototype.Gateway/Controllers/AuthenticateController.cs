using Microsoft.AspNetCore.Mvc;
using Prototype.Contracts;
using Prototype.Gateway.Services;

namespace Prototype.Gateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticateController : ControllerBase
{
    private readonly UserAuthService _userAuthService;

    public AuthenticateController(UserAuthService userAuthService)
    {
        _userAuthService = userAuthService;
    }

    [HttpPost]
    public async Task<IActionResult> Authenticate([FromBody] AuthRequest request)
    {
        try
        {
            var account = await _userAuthService.AuthenticateAsync(request.Provider, request.Token);
            return Ok(account);
        }
        catch (NotSupportedException ex)
        { 
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            // Log the exception
            return StatusCode(500, "An internal error occurred.");
        }
    }
}

public class AuthRequest
{
    public AuthProvider Provider { get; set; }
    public string Token { get; set; } = string.Empty;
}