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
    public IActionResult UserList()
    {
        var users = _context.Users.ToList(); // Fetch users from the database
        return View(users); // Pass data to the view
    }
}
