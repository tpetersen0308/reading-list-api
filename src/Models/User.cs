namespace reading_list_api.Models
{
  public class User
  {
    public System.Guid id { get; set; }
    public string email { get; set; }
    public string avatar { get; set; }
  }

  public class UserView
  {
    public string tokenId { get; set; }
  }
}