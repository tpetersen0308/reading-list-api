using reading_list_api.Models;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace test_reading_list_api.ModelsTests
{
  public class UserTests
  {
    [Fact]
    public void Add_user_to_database()
    {
      var options = new DbContextOptionsBuilder<ReadingListApiContext>()
      .UseInMemoryDatabase(databaseName: "reading_list_api")
      .Options;

      using (var context = new ReadingListApiContext(options))
      {
        context.Users.Add(new User
        {
          id = Guid.NewGuid(),
          email = "test email",
          avatar = "test avatar",
        });
        context.SaveChanges();

      }

      using (var context = new ReadingListApiContext(options))
      {
        Assert.Equal(1, context.Users.Count());
        Assert.Equal("test email", context.Users.Single().email);
      }
    }
  }
}