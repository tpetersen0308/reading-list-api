using reading_list_api.Models;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace test_reading_list_api.ModelsTests
{
  public class UserTests
  {
    [Fact]
    public void AddsUserToDatabase()
    {
      var options = new DbContextOptionsBuilder<ReadingListApiContext>()
      .UseInMemoryDatabase("adds_user_to_database")
      .Options;

      using (var context = new ReadingListApiContext(options))
      {
        context.Users.Add(new User
        {
          Email = "test email",
          Avatar = "test avatar",
        });
        context.SaveChanges();

        Assert.Equal(1, context.Users.Count());
        Assert.Equal("test email", context.Users.Last().Email);
      }
    }
  }
}