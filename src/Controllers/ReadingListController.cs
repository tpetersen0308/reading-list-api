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
    public ActionResult Get(Guid readingListId)
    {
      ReadingList readingList = LoadReadingList(readingListId);

      if (readingList == null)
      {
        return new NotFoundResult();
      }

      return Json(readingList);
    }

    [HttpPost]
    public JsonResult Post(ReadingList readingList)
    {
      User currentUser = _session.CurrentUser();
      readingList.Books[0].Ranking = 1;
      currentUser.ReadingLists.Add(readingList);
      _context.SaveChanges();

      return Json(readingList);
    }

    [HttpPut("{readingListId}")]
    public ActionResult Put(Guid readingListId, Book book)
    {
      ReadingList readingList = LoadReadingList(readingListId);

      if (readingList == null)
      {
        return new NotFoundResult();
      }

      book.Ranking = readingList.Books.Count() + 1;
      readingList.Books.Add(book);
      _context.SaveChanges();

      return Json(readingList);
    }

    [HttpPatch("{readingListId}")]
    public ActionResult Patch(Guid readingListId, PatchData patchData)
    {
      ReadingList readingList = LoadReadingList(readingListId);

      if (readingList == null)
      {
        return new NotFoundResult();
      }

      readingList.UpdateRankings(patchData.BookId, patchData.Ranking);
      _context.SaveChanges();

      return Json(readingList);
    }

    [HttpDelete("{readingListId}")]
    public ActionResult Delete(Guid readingListId)
    {
      ReadingList readingList = LoadReadingList(readingListId);

      if (readingList == null)
      {
        return new NotFoundResult();
      }

      _context.ReadingLists.Remove(readingList);
      _context.SaveChanges();

      return Json(NoContent());
    }

    [HttpDelete("{readingListId}/{bookId}")]
    public ActionResult Delete(Guid readingListId, Guid bookId)
    {
      ReadingList readingList = LoadReadingList(readingListId);

      if (readingList == null)
      {
        return new NotFoundResult();
      }

      readingList.RemoveBook(bookId);
      _context.SaveChanges();

      return Json(readingList);
    }

    private ReadingList LoadReadingList(Guid readingListId)
    {
      Guid currentUserId = _session.CurrentUser().UserId;
      return _context.ReadingLists
      .Where(r => r.ReadingListId == readingListId)
      .Where(r => r.UserId == currentUserId)
      .Include(r => r.Books)
      .FirstOrDefault();
    }

    private Boolean ReadingListBelongsToCurrentUser(Guid ownerId)
    {
      return ownerId == _session.CurrentUser().UserId;
    }
  }
}