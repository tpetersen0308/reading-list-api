using reading_list_api.Models;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

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

        context.ReadingLists.Add(new ReadingList
        {
          Title = "Empty Reading List",
        });
        context.SaveChanges();


        ReadingList readingList = context.ReadingLists.Last();
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
        Book book1 = new Book
        {
          Title = "On The Plurality Of Worlds",
          Authors = new string[] { "Rudolf Carnap" },
          Image = "test image url"
        };

        Book book2 = new Book
        {
          Title = "An Inquiry Concerning The Principles Of Morals",
          Authors = new string[] { "David Hume" },
          Image = "test image url"
        };

        Book book3 = new Book
        {
          Title = "Critique Of Pure Reason",
          Authors = new string[] { "Immanuel Kant" },
          Image = "test image url"
        };

        context.Books.AddRange(new List<Book>() { book1, book2, book3 });
        context.SaveChanges();

        context.ReadingLists.Add(new ReadingList
        {
          Title = "Existential Meltdown",
          Books = context.Books.ToList()
        });
        context.SaveChanges();

        ReadingList readingList = context.ReadingLists.Last();
        Assert.Equal(3, readingList.Books.Count());
      }
    }
  }
}