using reading_list_api.Models;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using test_reading_list_api.Fixtures;

namespace test_reading_list_api.ModelsTests
{
  public class ReadingListTests
  {
    [Fact]
    public void BelongsToUser()
    {
      var options = new DbContextOptionsBuilder<ReadingListApiContext>()
      .UseInMemoryDatabase("belongs_to_user")
      .Options;

      using (var context = new ReadingListApiContext(options))
      {
        context.Users.Add(new User
        {
          Email = "test email",
          Avatar = "test avatar",
        });
        context.SaveChanges();

        ReadingList readingList = new ReadingListFixture().ReadingList();
        context.ReadingLists.Add(readingList);

        User user = context.Users.Last();
        user.ReadingLists.Add(readingList);
        context.SaveChanges();

        Assert.Equal(user.UserId, readingList.User.UserId);
      }
    }

    [Fact]
    public void HasManyBooks()
    {
      var options = new DbContextOptionsBuilder<ReadingListApiContext>()
      .UseInMemoryDatabase("has_many_books")
      .Options;

      using (var context = new ReadingListApiContext(options))
      {
        context.ReadingLists.Add(new ReadingListFixture().ReadingList());
        context.SaveChanges();

        ReadingList readingList = context.ReadingLists.Last();
        Assert.Equal(3, readingList.Books.Count());
      }
    }
  }
}