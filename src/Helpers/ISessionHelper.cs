using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using reading_list_api.Models;
using Microsoft.AspNetCore.Http;

namespace reading_list_api.Helpers
{
  public interface ISessionHelper
  {
    User CurrentUser();
  }

  public class SessionHelper : ISessionHelper
  {
    private readonly ReadingListApiContext _dbContext;
    private readonly IHttpContextAccessor _contextAccessor;

    public SessionHelper(ReadingListApiContext dbContext, IHttpContextAccessor contextAccessor)
    {
      this._dbContext = dbContext;
      this._contextAccessor = contextAccessor;
    }

    public User CurrentUser()
    {
      string userId = _contextAccessor.HttpContext.Session.GetString("userId");
      User currentUser = _dbContext.Users
      .Where(u => u.UserId == Guid.Parse(userId))
      .Include(u => u.ReadingLists)
      .FirstOrDefault();
      return currentUser;
    }
  }
}