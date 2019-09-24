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
        context.Users.Add(new UserFixture().User());
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

    [Fact]
    public void CanDemoteBookRanking()
    {
      var options = new DbContextOptionsBuilder<ReadingListApiContext>()
      .UseInMemoryDatabase("can_demote_book_ranking")
      .Options;

      using (var context = new ReadingListApiContext(options))
      {
        ReadingList readingList = new ReadingListFixture().ReadingList();
        context.Add(readingList);
        context.SaveChanges();
        Book book1 = readingList.Books[0];
        Book book2 = readingList.Books[1];
        Book book3 = readingList.Books[2];
        readingList.UpdateRankings(book1.BookId, 2);

        Assert.Equal(2, book1.Ranking);
        Assert.Equal(1, book2.Ranking);
        Assert.Equal(3, book3.Ranking);
      }
    }

    [Fact]
    public void CanPromoteBookRanking()
    {
      var options = new DbContextOptionsBuilder<ReadingListApiContext>()
      .UseInMemoryDatabase("can_promote_book_ranking")
      .Options;

      using (var context = new ReadingListApiContext(options))
      {
        ReadingList readingList = new ReadingListFixture().ReadingList();
        context.Add(readingList);
        context.SaveChanges();
        Book book1 = readingList.Books[0];
        Book book2 = readingList.Books[1];
        Book book3 = readingList.Books[2];
        readingList.UpdateRankings(book3.BookId, 2);

        Assert.Equal(1, book1.Ranking);
        Assert.Equal(3, book2.Ranking);
        Assert.Equal(2, book3.Ranking);
      }
    }
  }
}