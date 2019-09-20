using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using reading_list_api.Models;
using reading_list_api.Helpers;
using reading_list_api.Services;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace reading_list_api.Controllers
{
  [Route("api/[controller]")]
  public class AuthController : Controller
  {
    private readonly IAuthService _authService;
    private readonly ISessionService _sessionService;

    public AuthController(IAuthService authService, ISessionService sessionService)
    {
      this._authService = authService;
      this._sessionService = sessionService;
    }

    [HttpPost("google")]
    public async Task<IActionResult> Google([FromBody]UserView userView)
    {
      try
      {
        Payload payload = ValidateAsync(userView.TokenId, new ValidationSettings()).Result;
        User user = _authService.Authenticate(payload);
        SimpleLogger.Log(payload.ExpirationTimeSeconds.ToString());

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
        {
          new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        }, "Cookies");

        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await Request.HttpContext.SignInAsync("Cookies", claimsPrincipal);
        HttpContext.Session.SetString("userId", user.UserId.ToString());
        HttpContext.Session.GetString("userId");


        return Ok(new
        {
          user = _sessionService.CurrentUser()
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
      HttpContext.Session.Remove("userId");
      await HttpContext.SignOutAsync();
      return NoContent();
    }
  }
}