using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using TheWall.Models;

namespace TheWall.Controllers;

[SessionCheck]
public class WallController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;

    public WallController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    //! Home View -> Show All Messages
    [HttpGet("messages")]
    public ViewResult AllMessages()
    {

        List<Message> AllMessages = _context.Messages.Include(m => m.MessageComments).ThenInclude(c => c.Commenter).Include(m => m.Creator).OrderByDescending(m => m.CreatedAt).ToList();
        return View("TheWall", AllMessages);
    }

    //! Create Message
    [HttpPost("messages/create")]
    public IActionResult CreateMessage(Message newMessage)
    {
        if (!ModelState.IsValid)
        {
            List<Message> AllMessages = _context.Messages.Include(m => m.MessageComments).ThenInclude(c => c.Commenter).Include(m => m.Creator).OrderByDescending(m => m.CreatedAt).ToList();
            return View("TheWall", AllMessages);
        }

        _context.Add(newMessage);
        _context.SaveChanges();
        return RedirectToAction("AllMessages");
    }

    //! Delete Message
    [HttpPost("messages/{messageId}/delete")]
    public IActionResult DeleteMessage(int messageId)
    {
        Message? MessageToDelete = _context.Messages.SingleOrDefault(m => m.MessageId == messageId);
        _context.Messages.Remove(MessageToDelete);
        _context.SaveChanges();
        return RedirectToAction("AllMessages");
    }

    //! Create Comment
    [HttpPost("comments/create")]
    public IActionResult CreateComment(Comment newComment)
    {
        if (!ModelState.IsValid)
        {
            List<Message> AllMessages = _context.Messages.Include(m => m.MessageComments).ThenInclude(c => c.Commenter).Include(m => m.Creator).OrderByDescending(m => m.CreatedAt).ToList();
            return View("TheWall", AllMessages);
        }

        _context.Add(newComment);
        _context.SaveChanges();
        return RedirectToAction("AllMessages");
    }

    //! Delete Comment
    [HttpPost("comments/{commentId}/delete")]
    public IActionResult DeleteComment(int commentId)
    {
        Comment? CommentToDelete = _context.Comments.SingleOrDefault(c => c.CommentId == commentId);
        _context.Comments.Remove(CommentToDelete);
        _context.SaveChanges();
        return RedirectToAction("AllMessages");
    }

}

//! Check if User is in Session
public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {

        int? userId = context.HttpContext.Session.GetInt32("UserId");

        if (userId == null)
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}