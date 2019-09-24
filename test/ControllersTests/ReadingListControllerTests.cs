using System;
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
    public void ReturnsAReadingList()
    {
      var options = new DbContextOptionsBuilder<ReadingListApiContext>()
      .UseInMemoryDatabase("returns_a_reading_list")
      .Options;

      using (var context = new ReadingListApiContext(options))
      {
        User user = new UserFixture().User();
        ReadingList readingList = new ReadingListFixture().ReadingList();
        user.ReadingLists.Add(readingList);
        context.Users.Add(user);
        context.SaveChanges();

        SessionHelperStub session = new SessionHelperStub(user);
        ReadingListController controller = new ReadingListController(context, session);


        JsonResult result = controller.Get(readingList.ReadingListId) as JsonResult;

        Assert.Equal(readingList, result.Value);
      }
    }

    [Fact]
    public void ShowReturns401WhenUserDoesntOwnList()
    {
      var options = new DbContextOptionsBuilder<ReadingListApiContext>()
      .UseInMemoryDatabase("show_returns_401")
      .Options;

      using (var context = new ReadingListApiContext(options))
      {
        ReadingList readingList = new ReadingListFixture().ReadingList();
        User user = new UserFixture().User();
        User unauthorizedUser = new User
        {
          Email = "unauthorized test email",
          Avatar = "unauthorized test avatar",
        };
        user.ReadingLists.Add(readingList);
        context.Users.AddRange(new List<User>() { user, unauthorizedUser });
        context.SaveChanges();

        SessionHelperStub session = new SessionHelperStub(unauthorizedUser);
        ReadingListController controller = new ReadingListController(context, session);

        JsonResult result = controller.Get(readingList.ReadingListId) as JsonResult;

        Assert.IsType<UnauthorizedResult>(result.Value);
      }
    }

    [Fact]
    public void CreatesNewReadingList()
    {
      var options = new DbContextOptionsBuilder<ReadingListApiContext>()
      .UseInMemoryDatabase("returns_users_reading_list")
      .Options;

      using (var context = new ReadingListApiContext(options))
      {
        User user = new UserFixture().User();
        context.Users.Add(user);
        context.SaveChanges();

        SessionHelperStub session = new SessionHelperStub(user);
        ReadingListController controller = new ReadingListController(context, session);

        ReadingList readingList = new ReadingList
        {
          Title = "Existential Meltdown",
          Books = new List<Book>() {
            new BookFixture().Book()
          }
        };

        JsonResult result = controller.Post(readingList) as JsonResult;

        Assert.Equal(1, context.ReadingLists.Count());
        Assert.Equal(1, context.Books.Count());
        Assert.Equal(readingList, result.Value);
        Assert.Equal(1, readingList.Books[0].Ranking);
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
        User user = new UserFixture().User();
        user.ReadingLists.Add(readingList);
        context.Users.Add(user);
        context.SaveChanges();

        SessionHelperStub session = new SessionHelperStub(user);
        ReadingListController controller = new ReadingListController(context, session);

        Book newBook = new BookFixture().Book();

        JsonResult result = controller.Put(readingList.ReadingListId, newBook) as JsonResult;
        List<Book> expectedResult = context.ReadingLists
        .Where(r => r.ReadingListId == readingList.ReadingListId)
        .FirstOrDefault()
        .Books;

        Assert.Equal(readingList, result.Value);
        Assert.Equal(4, context.Books.Count());
        Assert.Contains(newBook, expectedResult);
        Assert.Equal(4, readingList.Books[3].Ranking);
      }
    }

    [Fact]
    public void CreateReturns401WhenUserDoesntOwnList()
    {
      var options = new DbContextOptionsBuilder<ReadingListApiContext>()
      .UseInMemoryDatabase("create_returns_401")
      .Options;

      using (var context = new ReadingListApiContext(options))
      {
        ReadingList readingList = new ReadingListFixture().ReadingList();
        User user = new UserFixture().User();
        User unauthorizedUser = new User
        {
          Email = "unauthorized test email",
          Avatar = "unauthorized test avatar",
        };
        user.ReadingLists.Add(readingList);
        context.Users.AddRange(new List<User>() { user, unauthorizedUser });
        context.SaveChanges();

        SessionHelperStub session = new SessionHelperStub(unauthorizedUser);
        ReadingListController controller = new ReadingListController(context, session);

        Book newBook = new BookFixture().Book();

        JsonResult result = controller.Put(readingList.ReadingListId, newBook) as JsonResult;

        Assert.IsType<UnauthorizedResult>(result.Value);
      }
    }

    [Fact]
    public void PatchReordersAList()
    {
      var options = new DbContextOptionsBuilder<ReadingListApiContext>()
      .UseInMemoryDatabase("patch_reorders_a_list")
      .Options;

      using (var context = new ReadingListApiContext(options))
      {
        ReadingList readingList = new ReadingListFixture().ReadingList();
        User user = new UserFixture().User();
        user.ReadingLists.Add(readingList);
        context.Add(user);
        context.SaveChanges();

        Book book1 = readingList.Books[0];
        Book book2 = readingList.Books[1];
        Book book3 = readingList.Books[2];

        SessionHelperStub session = new SessionHelperStub(user);
        ReadingListController controller = new ReadingListController(context, session);

        PatchData data = new PatchData
        {
          BookId = book1.BookId,
          Ranking = 2
        };

        JsonResult result = controller.Patch(readingList.ReadingListId, data) as JsonResult;

        Assert.Equal(2, book1.Ranking);
        Assert.Equal(1, book2.Ranking);
        Assert.Equal(3, book3.Ranking);
      }
    }

    [Fact]
    public void PatchReturns401WhenUserDoesntOwnList()
    {
      var options = new DbContextOptionsBuilder<ReadingListApiContext>()
      .UseInMemoryDatabase("patch_returns_401")
      .Options;

      using (var context = new ReadingListApiContext(options))
      {
        ReadingList readingList = new ReadingListFixture().ReadingList();
        User user = new UserFixture().User();
        User unauthorizedUser = new User
        {
          Email = "unauthorized test email",
          Avatar = "unauthorized test avatar",
        };
        user.ReadingLists.Add(readingList);
        context.Users.AddRange(new List<User>() { user, unauthorizedUser });
        context.SaveChanges();

        SessionHelperStub session = new SessionHelperStub(unauthorizedUser);
        ReadingListController controller = new ReadingListController(context, session);

        PatchData data = new PatchData
        {
          BookId = readingList.Books[0].BookId,
          Ranking = 2
        };

        JsonResult result = controller.Patch(readingList.ReadingListId, data) as JsonResult;

        Assert.IsType<UnauthorizedResult>(result.Value);
      }
    }
  }
}