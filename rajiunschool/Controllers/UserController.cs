using System.Reflection.Metadata;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using rajiunschool.data;
using rajiunschool.Models;
public class UserController : Controller
{
    private readonly UmanagementContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public UserController(UmanagementContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
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
    public async Task<IActionResult> Profile()
    {
        int? userId = HttpContext.Session.GetInt32("userid");
        if (userId == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.id == userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        if (user.role.Equals("Student", StringComparison.OrdinalIgnoreCase))
        {
            var profile = await _context.ProfileStudents.FirstOrDefaultAsync(p => p.profileid == userId);
            if (profile == null) return NotFound("Student profile not found.");
            return View("StudentProfile", profile);
        }
        else
        {
            var profile = await _context.ProfileEmployees.FirstOrDefaultAsync(p => p.profileid == userId);
            if (profile == null) return NotFound("Employee profile not found.");
            return View("EmployeeProfile", profile);
        }
    }
    public async Task<IActionResult> EditProfileStudent()
    {
        int? userId = HttpContext.Session.GetInt32("userid");
        if (userId == null)
        {
            return RedirectToAction("Login", "Auth");  // If the user is not logged in, redirect to login page
        }

        var profile = await _context.ProfileStudents.FirstOrDefaultAsync(e => e.profileid == userId);
        if (profile == null)
        {
            return NotFound();  // If the student profile is not found, show an error
        }
        return View(profile);  // Pass the student profile to the EditProfile view
    }

    public async Task<IActionResult> EditProfileEmployee()
    {
        int? userId = HttpContext.Session.GetInt32("userid");
        if (userId == null)
        {
            return RedirectToAction("Login", "Auth");  // If the user is not logged in, redirect to login page
        }

        var profile = await _context.ProfileEmployees.FirstOrDefaultAsync(e => e.profileid == userId);
        if (profile == null)
        {
            return NotFound();  // If the teacher profile is not found, show an error
        }
        return View(profile);  // Pass the teacher profile to the EditProfile view
    }

    [HttpPost]
    public async Task<IActionResult> EditProfile(profilestudent studentModel, profileemployee employeeModel, IFormFile? profilePicture)
    {
        int? userId = HttpContext.Session.GetInt32("userid");
        if (userId == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        var role = HttpContext.Session.GetString("UserRole");

        if (role == "Student")
        {
            var profile = await _context.ProfileStudents.FirstOrDefaultAsync(e => e.profileid == userId);
            if (profile == null)
            {
                return NotFound();
            }

            // Update profile properties
            profile.name = studentModel?.name;

            // Handle profile picture upload
            if (profilePicture != null)
            {
                profile.ProfilePicture = await SaveProfilePicture(profilePicture);
            }

            _context.ProfileStudents.Update(profile);
        }
        else if (role == "Teacher" || role == "Employee") // Assuming employees include teachers
        {
            var profile = await _context.ProfileEmployees.FirstOrDefaultAsync(e => e.profileid == userId);
            if (profile == null)
            {
                return NotFound();
            }

            // Update profile properties
            profile.name = employeeModel?.name;
            profile.age = employeeModel?.age;
            profile.sex = employeeModel?.sex;
            profile.bloodgroup = employeeModel?.bloodgroup;
            profile.details = employeeModel?.details;

            // Handle profile picture upload
            if (profilePicture != null)
            {
                profile.ProfilePicture = await SaveProfilePicture(profilePicture);
            }

            _context.ProfileEmployees.Update(profile);
        }
        else
        {
            return NotFound("User role not found.");
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Profile");
    }
    // Helper function to save profile picture
    private async Task<string> SaveProfilePicture(IFormFile profilePicture)
    {
        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(profilePicture.FileName);
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await profilePicture.CopyToAsync(fileStream);
        }

        return "/images/" + uniqueFileName; // Save path in database
    }
    public IActionResult nextPage(String UserListnow)
    {
        if (UserListnow.Equals( "Teacher"))
            return View("nextPageTeacher");
         return View("nextPageStudent");
    }
}
