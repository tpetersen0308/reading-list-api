using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace reading_list_api.Models
{
  public class ReadingList
  {
    public ReadingList()
    {
      Books = new List<Book>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ReadingListId { get; set; }
    [Required]
    public string Title { get; set; }
    [MinLength(1)]
    public virtual List<Book> Books { get; set; }
    public Guid UserId { get; set; }
    [JsonIgnore]
    public virtual User User { get; set; }

    public ReadingList UpdateRankings(Guid bookId, int newRanking)
    {
      Book book = this.Books.Find(b => b.BookId == bookId);
      int oldRanking = book.Ranking;

      if (newRanking <= oldRanking)
      {
        PromoteBook(oldRanking, newRanking);
      }
      else
      {
        DemoteBook(oldRanking, newRanking);
      }
      book.Ranking = newRanking;

      return this;
    }
    private void DemoteBook(int oldRanking, int newRanking)
    {
      for (int i = oldRanking + 1; i <= newRanking; i++)
      {
        Book currentBook = this.Books.Find(b => b.Ranking == i);
        currentBook.Ranking = i - 1;
      }
    }

    private void PromoteBook(int oldRanking, int newRanking)
    {
      for (int i = oldRanking - 1; i >= newRanking; i--)
      {
        Book currentBook = this.Books.Find(b => b.Ranking == i);
        currentBook.Ranking = i + 1;
      }
    }
  }
}