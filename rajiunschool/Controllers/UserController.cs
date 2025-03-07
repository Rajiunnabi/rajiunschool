using System.Reflection.Metadata;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using rajiunschool.data;
using rajiunschool.Models;
using Microsoft.Extensions.Logging;

public class UserController : Controller
{
    private readonly UmanagementContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<UserController> _logger;

    public UserController(UmanagementContext context, IWebHostEnvironment webHostEnvironment, ILogger<UserController> logger)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
    }

    // GET: Display users in a table
    public IActionResult Student(string searchQuery)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Student action");
            return RedirectToAction("Error", "Home");
        }
    }

    public IActionResult Teacher(string searchQuery)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Teacher action");
            return RedirectToAction("Error", "Home");
        }
    }

    public IActionResult Libarian(string searchQuery)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Libarian action");
            return RedirectToAction("Error", "Home");
        }
    }

    public IActionResult Banker(string searchQuery)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Banker action");
            return RedirectToAction("Error", "Home");
        }
    }

    public IActionResult Others(string searchQuery)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Others action");
            return RedirectToAction("Error", "Home");
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.role.Equals("Student"))
            {
                var profile = await _context.ProfileStudents.FirstOrDefaultAsync(e => e.profileid == id);
                if (profile == null) return NotFound("Student profile not found.");
                return View("StudentDetails", profile);
            }
            else
            {
                var profile = await _context.ProfileEmployees.FirstOrDefaultAsync(e => e.profileid == id);
                if (profile == null) return NotFound("Employee profile not found.");
                return View("EmployeeDetails", profile);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Details action");
            return RedirectToAction("Error", "Home");
        }
    }

    public async Task<IActionResult> Profile()
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Profile action");
            return RedirectToAction("Error", "Home");
        }
    }

    public async Task<IActionResult> EditProfileStudent()
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in EditProfileStudent action");
            return RedirectToAction("Error", "Home");
        }
    }

    public async Task<IActionResult> EditProfileEmployee()
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in EditProfileEmployee action");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditProfile(profilestudent studentModel, profileemployee employeeModel, IFormFile? profilePicture)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in EditProfile action");
            return RedirectToAction("Error", "Home");
        }
    }

    // Helper function to save profile picture
    private async Task<string> SaveProfilePicture(IFormFile profilePicture)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in SaveProfilePicture method");
            throw; // Re-throw the exception to be handled by the calling method
        }
    }

    public IActionResult nextPage(String UserListnow)
    {
        try
        {
            if (UserListnow.Equals("Teacher"))
                return View("nextPageTeacher");
            return View("nextPageStudent");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in nextPage action");
            return RedirectToAction("Error", "Home");
        }
    }
    //Create a method name ChangeStatus which will change the value to 0 if the profileStudents.running = 1 and vice versa and also if the User.role in not student it will do the same in ProfileEmployee
    public async Task<IActionResult> ChangeStatus(int id,string role)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            if (role.Equals("Student"))
            {
                var profile = await _context.ProfileStudents.FirstOrDefaultAsync(e => e.profileid == id);
                if (profile == null) return NotFound("Student profile not found.");
                profile.running = profile.running == 1 ? 0 : 1;
                _context.ProfileStudents.Update(profile);
            }
            else
            {
                var profile = await _context.ProfileEmployees.FirstOrDefaultAsync(e => e.profileid == id);
                if (profile == null) return NotFound("Employee profile not found.");
                profile.running = profile.running == 1 ? 0 : 1;
                _context.ProfileEmployees.Update(profile);
            }
            //now it will search User.running and convert it to 0 if it is 1 and vice versa
            user.running = user.running == 1 ? 0 : 1;
            await _context.SaveChangesAsync();
            var userlist = _context.Users.Where(u => u.id == id).ToList();
            ViewData["UserListnow"] = role;
            return View("UserList",userlist);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ChangeStatus action");
            return RedirectToAction("Error", "Home");
        }
    }


}