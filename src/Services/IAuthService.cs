using reading_list_api.Models;
using System;
using System.Linq;

namespace reading_list_api.Services
{
  public interface IAuthService
  {
    User Authenticate(Google.Apis.Auth.GoogleJsonWebSignature.Payload payload);
  }

  public class AuthService : IAuthService
  {
    private readonly ReadingListApiContext _context;

    public AuthService(ReadingListApiContext context)
    {
      _context = context;
    }
    public User Authenticate(Google.Apis.Auth.GoogleJsonWebSignature.Payload payload)
    {
      return this.FindUserOrAdd(payload);
    }

    private User FindUserOrAdd(Google.Apis.Auth.GoogleJsonWebSignature.Payload payload)
    {
      var u = _context.Users.Where(x => x.email == payload.Email).FirstOrDefault();
      if (u == null)
      {
        u = new User()
        {
          id = Guid.NewGuid(),
          email = payload.Email,
          avatar = payload.Picture,
        };
        _context.Users.Add(u);
        _context.SaveChanges();
      }
      return u;
    }
  }
}