using System;
using System.Collections.Generic;

namespace reading_list_api.Models
{
  public class ReadingList
  {
    public Guid ReadingListId { get; set; }
    public string Title { get; set; }
    public virtual List<Book> Books { get; set; }
    public User User { get; set; }
    public ReadingList()
    {
      ReadingListId = System.Guid.NewGuid();
      Books = new List<Book>();
    }
  }
}