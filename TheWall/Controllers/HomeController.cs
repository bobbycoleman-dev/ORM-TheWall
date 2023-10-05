using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheWall.Models;

namespace TheWall.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    //! Index -> Login & Registrations Forms
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }

    //! Register User
    [HttpPost("users/create")]
    public IActionResult Register(User newUser)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }

        PasswordHasher<User> Hasher = new();
        newUser.Password = Hasher.HashPassword(newUser, newUser.Password);

        _context.Add(newUser);
        _context.SaveChanges();

        HttpContext.Session.SetInt32("UserId", newUser.UserId);
        HttpContext.Session.SetString("UserName", newUser.FirstName);
        return RedirectToAction("AllMessages", "Wall");
    }

    //! Login User
    [HttpPost("users/login")]
    public IActionResult Login(LoginUser loginForm)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }

        User? UserInDb = _context.Users.FirstOrDefault(u => u.Email == loginForm.LogEmail);

        if (UserInDb == null)
        {
            ModelState.AddModelError("LogPassword", "Invalid Email/Password");
            return View("Index");
        }

        PasswordHasher<LoginUser> Hasher = new();
        PasswordVerificationResult result = Hasher.VerifyHashedPassword(loginForm, UserInDb.Password, loginForm.LogPassword);

        if (result == 0)
        {
            ModelState.AddModelError("LogPassword", "Invalid Email/Password");
            return View("Index");
        }

        HttpContext.Session.SetInt32("UserId", UserInDb.UserId);
        HttpContext.Session.SetString("UserName", UserInDb.FirstName);
        return RedirectToAction("AllMessages", "Wall");
    }

    //! Logout User
    [HttpGet("users/logout")]
    public RedirectToActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

