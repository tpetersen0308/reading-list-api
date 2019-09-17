using System;
using reading_list_api.Models;
using Microsoft.AspNetCore.Http;

namespace reading_list_api.Services
{
  public interface ISessionService
  {
    User CurrentUser();
  }

  public class SessionService : ISessionService
  {
    private readonly ReadingListApiContext _dbContext;
    private readonly IHttpContextAccessor _contextAccessor;

    public SessionService(ReadingListApiContext dbContext, IHttpContextAccessor contextAccessor)
    {
      this._dbContext = dbContext;
      this._contextAccessor = contextAccessor;
    }

    public User CurrentUser()
    {
      string userId = _contextAccessor.HttpContext.Session.GetString("userId");
      User currentUser = _dbContext.Users.Find(Guid.Parse(userId));
      return currentUser;
    }
  }
}