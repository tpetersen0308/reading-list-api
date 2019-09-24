using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace reading_list_api.Models
{
  public class Book
  {
    public Book()
    {
      DateCreated = DateTime.Now;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid BookId { get; set; }
    public string Title { get; set; }
    public string Image { get; set; }
    public string[] Authors { get; set; }
    public int Ranking { get; set; }
    public Guid ReadingListId { get; set; }
    [JsonIgnore]
    public virtual ReadingList ReadingList { get; set; }
    public DateTime DateCreated { get; set; }
  }
}