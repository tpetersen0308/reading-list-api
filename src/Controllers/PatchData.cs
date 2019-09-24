using System;

namespace reading_list_api.Controllers
{
  public class PatchData
  {
    public Guid BookId { get; set; }
    public int Ranking { get; set; }
  }
}