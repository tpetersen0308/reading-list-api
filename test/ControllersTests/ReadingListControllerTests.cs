using reading_list_api.Models;
using reading_list_api.Controllers;
using test_reading_list_api.Stubs;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace test_reading_list_api.ControllersTests
{
  public class ReadingListControllerTests
  {
    [Fact]
    public void CreatesNewReadingListWithBookAndUser()
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

        SessionServiceStub sessionService = new SessionServiceStub(user);
        ReadingListController controller = new ReadingListController(context, sessionService);

        Assert.Equal(0, context.ReadingLists.Count());
        Book newBook = new Book
        {
          Title = "On The Plurality Of Worlds",
          Authors = new string[] { "Rudolf Carnap" },
          Image = "test image url"
        };

        ReadingList readingList = new ReadingList
        {
          Title = "Existential Meltdown",
          Books = new System.Collections.Generic.List<Book>() { newBook }
        };

        JsonResult result = controller.Post(readingList) as JsonResult;
        ReadingList expectedResult = context.ReadingLists.Last();

        Assert.Equal(1, context.ReadingLists.Count());
        Assert.Equal(1, context.Books.Count());
        Assert.Equal(user, expectedResult.User);
        Assert.Equal(expectedResult, context.Books.Last().ReadingList);
      }
    }
  }
}