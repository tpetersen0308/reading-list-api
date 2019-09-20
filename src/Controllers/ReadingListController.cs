using System;
using System.Linq;
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
      ReadingList newReadingList = _context.ReadingLists.Add(readingList).Entity;
      currentUser.ReadingLists.Add(newReadingList);
      _context.SaveChanges();

      return Json(newReadingList);
    }

    [HttpPut("{readingListId}")]
    public JsonResult Put(string readingListId, Book book)
    {
      ReadingList readingList = _context.ReadingLists
      .Where(r => r.ReadingListId == Guid.Parse(readingListId))
      .FirstOrDefault();

      if (readingList.UserId != _sessionService.CurrentUser().UserId)
      {
        return Json(Unauthorized());
      }

      readingList.Books.Add(book);
      _context.SaveChanges();

      return Json(readingList);
    }
  }
}