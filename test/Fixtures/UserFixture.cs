using reading_list_api.Models;

namespace test_reading_list_api.Fixtures
{
  public class UserFixture
  {
    public User User()
    {
      User user = new User
      {
        Email = "test email",
        Avatar = "test avatar"
      };
      return user;
    }
  }
}