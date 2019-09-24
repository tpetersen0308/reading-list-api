using System.Collections.Generic;
using reading_list_api.Models;

namespace test_reading_list_api.Fixtures
{

  public class ReadingListFixture
  {

    public ReadingList ReadingList()
    {
      Book book1 = new Book
      {
        Title = "On The Plurality Of Worlds",
        Authors = new string[] { "David Lewis" },
        Image = "test image url",
        Ranking = 1
      };

      Book book2 = new Book
      {
        Title = "An Inquiry Concerning The Principles Of Morals",
        Authors = new string[] { "David Hume" },
        Image = "test image url",
        Ranking = 2
      };

      Book book3 = new Book
      {
        Title = "Critique Of Pure Reason",
        Authors = new string[] { "Immanuel Kant" },
        Image = "test image url",
        Ranking = 3
      };

      ReadingList readingList = new ReadingList
      {
        Title = "Existential Meltdown",
        Books = new List<Book>() { book1, book2, book3 }
      };
      return readingList;
    }
  }
}