using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using reading_list_api.Models;
using reading_list_api.Helpers;
using reading_list_api.Services;
using Microsoft.AspNetCore.Authentication;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace reading_list_api.Controllers
{
  [Route("api/[controller]")]
  public class AuthController : Controller
  {
    private IAuthService _authService;

    public AuthController(IAuthService authService)
    {
      this._authService = authService;
    }

    [HttpPost("google")]
    public async Task<IActionResult> Google([FromBody]UserView userView)
    {
      try
      {
        Payload payload = ValidateAsync(userView.tokenId, new ValidationSettings()).Result;
        User user = _authService.Authenticate(payload);
        SimpleLogger.Log(payload.ExpirationTimeSeconds.ToString());

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
        {
          new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
        }, "Cookies");

        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await Request.HttpContext.SignInAsync("Cookies", claimsPrincipal);

        return Ok(new
        {
          user = user
        });
      }
      catch (Exception ex)
      {
        BadRequest(ex.Message);
      }
      return BadRequest();
    }

    [HttpGet("signout")]
    public async Task<IActionResult> Logout()
    {
      await HttpContext.SignOutAsync();
      return NoContent();
    }
  }
}