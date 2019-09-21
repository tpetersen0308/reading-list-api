using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using reading_list_api.Models;
using reading_list_api.Helpers;

namespace reading_list_api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class ReadingListController : Controller
  {
    private readonly ReadingListApiContext _context;
    private readonly ISessionHelper _session;

    public ReadingListController(ReadingListApiContext context, ISessionHelper session)
    {
      _context = context;
      _session = session;
    }

    [HttpGet("{readingListId}")]
    public JsonResult Get(string readingListId)
    {
      ReadingList readingList = _context.ReadingLists
      .Where(r => r.ReadingListId == Guid.Parse(readingListId))
      .Include(r => r.Books)
      .FirstOrDefault();

      if (!readingListBelongsToCurrentUser(readingList.UserId))
      {
        return Json(Unauthorized());
      }

      return Json(readingList);
    }

    [HttpPost]
    public JsonResult Post(ReadingList readingList)
    {
      User currentUser = _session.CurrentUser();
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

      if (!readingListBelongsToCurrentUser(readingList.UserId))
      {
        return Json(Unauthorized());
      }

      readingList.Books.Add(book);
      _context.SaveChanges();

      return Json(readingList);
    }

    private Boolean readingListBelongsToCurrentUser(Guid ownerId)
    {
      return ownerId == _session.CurrentUser().UserId;
    }

  }
}