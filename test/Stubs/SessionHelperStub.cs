using reading_list_api.Helpers;
using reading_list_api.Models;

namespace test_reading_list_api.Stubs
{
  public class SessionHelperStub : ISessionHelper
  {
    private readonly User _user;
    public SessionHelperStub(User user)
    {
      this._user = user;
    }
    public User CurrentUser()
    {
      return _user;
    }
  }
}