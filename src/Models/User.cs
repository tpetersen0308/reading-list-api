using System;
using System.Collections.Generic;

namespace reading_list_api.Models
{
  public class User
  {
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