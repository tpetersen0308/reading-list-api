using reading_list_api.Services;
using reading_list_api.Models;

namespace test_reading_list_api.Stubs
{
  public class SessionServiceStub : ISessionService
  {
    private readonly User _user;
    public SessionServiceStub(User user)
    {
      this._user = user;
    }
    public User CurrentUser()
    {
      return _user;
    }
  }
}