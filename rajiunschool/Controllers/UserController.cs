using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    public IActionResult Student(string searchQuery)
    {
        ViewData["UserListnow"] = "Student";
    
        var users = _context.Users
            .Where(u => u.role == "Student")
            .ToList(); // Fetch all students by default

        if (!string.IsNullOrEmpty(searchQuery))
        {
            users = users.Where(u =>
                u.id.ToString().Contains(searchQuery) || 
                u.username.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
            .ToList();
        }

        return View("Userlist", users); // Pass filtered data to the view
    }
    public IActionResult Teacher(string searchQuery)
    {
        ViewData["UserListnow"] = "Teacher";

        var teachers = _context.Users
            .Where(u => u.role == "Teacher")
            .ToList(); // Fetch all teachers by default

        if (!string.IsNullOrEmpty(searchQuery))
        {
            teachers = teachers.Where(u =>
                u.id.ToString().Contains(searchQuery) ||
                u.username.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
            .ToList();
        }

        return View("Userlist", teachers); // Pass filtered or full list to the view
    }

    public IActionResult Libarian(string searchQuery)
    {
        ViewData["UserListnow"] = "Libarian";

        var librarians = _context.Users
            .Where(u => u.role == "Libarian")
            .ToList(); // Fetch all librarians by default

        if (!string.IsNullOrEmpty(searchQuery))
        {
            librarians = librarians.Where(u =>
                u.id.ToString().Contains(searchQuery) ||
                u.username.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
            .ToList();
        }

        return View("Userlist", librarians); // Pass filtered or full list to the view
    }


    public IActionResult Banker(string searchQuery)
    {
        ViewData["UserListnow"] = "Banker";

        var bankers = _context.Users
            .Where(u => u.role == "Banker")
            .ToList(); // Fetch all bankers by default

        if (!string.IsNullOrEmpty(searchQuery))
        {
            bankers = bankers.Where(u =>
                u.id.ToString().Contains(searchQuery) ||
                u.username.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
            .ToList();
        }

        return View("Userlist", bankers); // Pass filtered or full list to the view
    }

    public IActionResult Others(string searchQuery)
    {
        ViewData["UserListnow"] = "Others";

        var others = _context.Users
            .Where(u => u.role == "Others")
            .ToList(); // Fetch all users in "Others" category by default

        if (!string.IsNullOrEmpty(searchQuery))
        {
            others = others.Where(u =>
                u.id.ToString().Contains(searchQuery) ||
                u.username.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
            .ToList();
        }

        return View("Userlist", others); // Pass filtered or full list to the view
    }

    public async Task<IActionResult> Details(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user.role.Equals("Student"))
        {
            var profile= await _context.ProfileStudents.FirstOrDefaultAsync(e => e.profileid == id);
            return View("StudentDetails",profile);

        }
        else
        {
            var profile = await _context.ProfileEmployees.FirstOrDefaultAsync(e => e.profileid == id);
            return View("EmployeeDetails",profile);

        }

    }
    public IActionResult nextPage(String UserListnow)
    {
        if (UserListnow.Equals( "Teacher"))
            return View("nextPageTeacher");
         return View("nextPageStudent");
    }

   

    }
