using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using reading_list_api.Models;
using reading_list_api.Services;

namespace reading_list_api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class ReadingListController : Controller
  {
    private readonly ReadingListApiContext _context;
    private readonly ISessionService _sessionService;

    public ReadingListController(ReadingListApiContext context, ISessionService sessionService)
    {
      _context = context;
      _sessionService = sessionService;
    }

    [HttpPost]
    public JsonResult Post(ReadingList readingList)
    {
      User currentUser = _sessionService.CurrentUser();
      ReadingList newReadingList = new ReadingList
      {
        Title = readingList.Title,
        User = currentUser
      };
      _context.ReadingLists.Add(newReadingList);

      Book newBook = new Book
      {
        Title = readingList.Books[0].Title,
        Authors = readingList.Books[0].Authors,
        Image = readingList.Books[0].Image,
        ReadingList = newReadingList
      };
      _context.Books.Add(newBook);
      _context.SaveChanges();


      return Json(newReadingList);
    }
  }
}