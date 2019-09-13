using System;
using Microsoft.AspNetCore.Mvc;
using reading_list_api.Models;
using reading_list_api.Helpers;
using reading_list_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Google.Apis.Auth;

namespace reading_list_api.Controllers
{
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [Route("api/[controller]")]
  public class AuthController : Controller
  {
    private IAuthService _authService;

    public AuthController(IAuthService authService)
    {
      this._authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("google")]
    public IActionResult Google([FromBody]UserView userView)
    {
      try
      {
        var payload = GoogleJsonWebSignature.ValidateAsync(userView.tokenId, new GoogleJsonWebSignature.ValidationSettings()).Result;
        User user = _authService.Authenticate(payload);
        SimpleLogger.Log(payload.ExpirationTimeSeconds.ToString());
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
  }
}