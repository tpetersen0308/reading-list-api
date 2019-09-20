using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace reading_list_api.Models
{
  public class User
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string Avatar { get; set; }
    public virtual List<ReadingList> ReadingLists { get; set; }

    public User()
    {
      UserId = Guid.NewGuid();
      ReadingLists = new List<ReadingList>();
    }
  }

  public class UserView
  {
    public string TokenId { get; set; }
  }
}