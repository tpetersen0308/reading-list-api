using reading_list_api.Services;
using reading_list_api.Models;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace test_reading_list_api.ServicesTests
{
  public class IAuthServiceTest
  {
    [Fact]
    public void AddsUserToDatabase()
    {
      var options = new DbContextOptionsBuilder<ReadingListApiContext>()
      .UseInMemoryDatabase("adds_user_to_database")
      .Options;

      using (var context = new ReadingListApiContext(options))
      {
        Payload payload = new Payload
        {
          Email = "test email",
          Picture = "test picture",
        };

        AuthService auth = new AuthService(context);

        Assert.Equal(1, context.Users.Count());
        Assert.Equal("test email", context.Users.Last().Email);
      }
    }
  }
}