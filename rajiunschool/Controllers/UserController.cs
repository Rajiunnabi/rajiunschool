using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rajiunschool.data;
using rajiunschool.Models;
public class UserController : Controller
{
    private readonly UmanagementContext _context;

    public UserController(UmanagementContext context)
    {
        _context = context;
    }

    // GET: Display users in a table
    public IActionResult Student()
    {
        ViewData["UserListnow"] = "Student";
        var users = _context.Users.ToList(); // Fetch users from the database
        return View("Userlist", users);// Pass data to the view
    }

    public IActionResult Teacher()
    {
        ViewData["UserListnow"] = "Teacher";
        var users = _context.Users.ToList(); // Fetch users from the database
        return View("Userlist", users);// Pass data to the view
    }
    public IActionResult Libarian()
    {
        ViewData["UserListnow"] = "Libarian";
        var users = _context.Users.ToList(); // Fetch users from the database
        return View("Userlist", users);// Pass data to the view
    }

    public IActionResult Banker()
    {
        ViewData["UserListnow"] = "Banker";
        var users = _context.Users.ToList(); // Fetch users from the database
        return View("Userlist", users);// Pass data to the view
    }
    public IActionResult Others()
    {
        ViewData["UserListnow"] = "Others";
        var users = _context.Users.ToList(); // Fetch users from the database
        return View("Userlist", users);// Pass data to the view
    }
}
