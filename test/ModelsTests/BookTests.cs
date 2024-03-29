using reading_list_api.Models;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using test_reading_list_api.Fixtures;

namespace test_reading_list_api.ModelsTests
{
  public class BookTests
  {
    [Fact]
    public void AddsBookToDatabase()
    {
      var options = new DbContextOptionsBuilder<ReadingListApiContext>()
      .UseInMemoryDatabase("adds_books_to_database")
      .Options;

      using (var context = new ReadingListApiContext(options))
      {
        context.Books.Add(new BookFixture().Book());

        context.SaveChanges();

        Book book = context.Books.Last();
        Assert.Equal(1, context.Books.Count());
        Assert.Equal("On The Plurality Of Worlds", book.Title);
        Assert.Equal("David Lewis", book.Authors[0]);
      }
    }

    [Fact]
    public void BelongsToReadingList()
    {
      var options = new DbContextOptionsBuilder<ReadingListApiContext>()
      .UseInMemoryDatabase("belongs_to_reading_list")
      .Options;

      using (var context = new ReadingListApiContext(options))
      {
        context.ReadingLists.Add(new ReadingListFixture().ReadingList());
        context.SaveChanges();

        Book book = context.Books.Last();
        ReadingList readingList = context.ReadingLists.Last();
        Assert.Equal(book.ReadingList.Title, readingList.Title);
      }
    }
  }
}