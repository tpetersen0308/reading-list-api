using System.Collections.Generic;
using reading_list_api.Models;

namespace test_reading_list_api.Fixtures
{

  public class BookFixture
  {

    public Book Book()
    {
      Book book = new Book
      {
        Title = "On The Plurality Of Worlds",
        Authors = new string[] { "David Lewis" },
        Image = "test image url"
      };


      return book;
    }
  }
}