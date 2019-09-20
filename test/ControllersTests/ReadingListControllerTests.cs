using reading_list_api.Models;
using reading_list_api.Controllers;
using test_reading_list_api.Stubs;
using test_reading_list_api.Fixtures;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace test_reading_list_api.ControllersTests
{
  public class ReadingListControllerTests
  {
    [Fact]
    public void CreatesNewReadingList()
    {
      var options = new DbContextOptionsBuilder<ReadingListApiContext>()
      .UseInMemoryDatabase("returns_users_reading_list")
      .Options;

      using (var context = new ReadingListApiContext(options))
      {
        User user = new User
        {
          Email = "test email",
          Avatar = "test avatar"
        };
        context.Users.Add(user);
        context.SaveChanges();

        SessionServiceStub sessionService = new SessionServiceStub(user);
        ReadingListController controller = new ReadingListController(context, sessionService);

        ReadingList readingList = new ReadingListFixture().ReadingList();

        JsonResult result = controller.Post(readingList) as JsonResult;

        Assert.Equal(1, context.ReadingLists.Count());
        Assert.Equal(3, context.Books.Count());
        Assert.Equal(readingList, result.Value);
      }
    }

    [Fact]
    public void AddsBooksToExistingReadingList()
    {
      var options = new DbContextOptionsBuilder<ReadingListApiContext>()
      .UseInMemoryDatabase("adds_book_to_existing_reading_list")
      .Options;

      using (var context = new ReadingListApiContext(options))
      {
        ReadingList readingList = new ReadingListFixture().ReadingList();
        User user = new User
        {
          Email = "test email",
          Avatar = "test avatar",
        };
        user.ReadingLists.Add(readingList);
        context.Users.Add(user);
        context.SaveChanges();

        SessionServiceStub sessionService = new SessionServiceStub(user);
        ReadingListController controller = new ReadingListController(context, sessionService);

        Book newBook = new Book
        {
          Title = "The Philosophical Foundations of Physics",
          Authors = new string[] { "Rudolf Carnap" },
          Image = "test image url"
        };

        JsonResult result = controller.Put(readingList.ReadingListId.ToString(), newBook) as JsonResult;
        List<Book> expectedResult = context.ReadingLists
        .Where(r => r.ReadingListId == readingList.ReadingListId)
        .FirstOrDefault()
        .Books;

        Assert.Equal(readingList, result.Value);
        Assert.Equal(4, context.Books.Count());
        Assert.Contains(newBook, expectedResult);
      }
    }

    [Fact]
    public void Returns401WhenUserDoesntOwnList()
    {
      var options = new DbContextOptionsBuilder<ReadingListApiContext>()
      .UseInMemoryDatabase("adds_book_to_existing_reading_list")
      .Options;

      using (var context = new ReadingListApiContext(options))
      {
        ReadingList readingList = new ReadingListFixture().ReadingList();
        User user = new User
        {
          Email = "test email",
          Avatar = "test avatar",
        };
        User unauthorizedUser = new User
        {
          Email = "unauthorized test email",
          Avatar = "unauthorized test avatar",
        };
        user.ReadingLists.Add(readingList);
        context.Users.AddRange(new List<User>() { user, unauthorizedUser });
        context.SaveChanges();

        SessionServiceStub sessionService = new SessionServiceStub(unauthorizedUser);
        ReadingListController controller = new ReadingListController(context, sessionService);

        Book newBook = new Book
        {
          Title = "The Philosophical Foundations of Physics",
          Authors = new string[] { "Rudolf Carnap" },
          Image = "test image url"
        };

        JsonResult result = controller.Put(readingList.ReadingListId.ToString(), newBook) as JsonResult;

        Assert.IsType<UnauthorizedResult>(result.Value);
      }
    }
  }
}