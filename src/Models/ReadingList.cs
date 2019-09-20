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
    public string Title { get; set; }
    public virtual List<Book> Books { get; set; }
    public Guid UserId { get; set; }
    [JsonIgnore]
    public virtual User User { get; set; }
  }
}