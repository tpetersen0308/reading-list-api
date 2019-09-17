using System;

namespace reading_list_api.Models
{
  public class Book
  {
    public Guid BookId { get; set; }
    public string Title { get; set; }
    public string Image { get; set; }
    public string[] Authors { get; set; }
    public ReadingList ReadingList { get; set; }
    public DateTime DateCreated { get; set; }
    public Book()
    {
      BookId = Guid.NewGuid();
      DateCreated = DateTime.Now;
    }
  }
}