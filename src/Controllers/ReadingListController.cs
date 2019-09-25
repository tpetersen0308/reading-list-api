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
    public JsonResult Get(Guid readingListId)
    {
      ReadingList readingList = LoadReadingList(readingListId);

      if (!ReadingListBelongsToCurrentUser(readingList.UserId))
      {
        return Json(Unauthorized());
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
    public JsonResult Put(Guid readingListId, Book book)
    {
      ReadingList readingList = LoadReadingList(readingListId);

      if (!ReadingListBelongsToCurrentUser(readingList.UserId))
      {
        return Json(Unauthorized());
      }

      book.Ranking = readingList.Books.Count() + 1;
      readingList.Books.Add(book);
      _context.SaveChanges();

      return Json(readingList);
    }

    [HttpPatch("{readingListId}")]
    public JsonResult Patch(Guid readingListId, PatchData patchData)
    {
      ReadingList readingList = LoadReadingList(readingListId);

      if (!ReadingListBelongsToCurrentUser(readingList.UserId))
      {
        return Json(Unauthorized());
      }

      readingList.UpdateRankings(patchData.BookId, patchData.Ranking);
      _context.SaveChanges();

      return Json(readingList);
    }

    [HttpDelete("{readingListId}")]
    public JsonResult Delete(Guid readingListId)
    {
      ReadingList readingList = LoadReadingList(readingListId);

      if (!ReadingListBelongsToCurrentUser(readingList.UserId))
      {
        return Json(Unauthorized());
      }

      _context.ReadingLists.Remove(readingList);
      _context.SaveChanges();

      return Json(NoContent());
      {

      }
    }

    [HttpDelete("{readingListId}/{bookId}")]
    public JsonResult Delete(Guid readingListId, Guid bookId)
    {
      ReadingList readingList = LoadReadingList(readingListId);

      if (!ReadingListBelongsToCurrentUser(readingList.UserId))
      {
        return Json(Unauthorized());
      }

      readingList.RemoveBook(bookId);
      _context.SaveChanges();

      return Json(readingList);
    }

    private ReadingList LoadReadingList(Guid readingListId)
    {
      return _context.ReadingLists
      .Where(r => r.ReadingListId == readingListId)
      .Include(r => r.Books)
      .FirstOrDefault();
    }

    private Boolean ReadingListBelongsToCurrentUser(Guid ownerId)
    {
      return ownerId == _session.CurrentUser().UserId;
    }
  }
}