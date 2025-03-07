using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using rajiunschool.data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace rajiunschool.Controllers
{
    public class AuthController : Controller
    {
        private readonly UmanagementContext _context;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UmanagementContext context, ILogger<AuthController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Show Login Page
        public IActionResult Login()
        {
            return View();
        }

        // POST: Process Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    ViewBag.Error = "Username and password are required.";
                    return View();
                }

                // Fetch the latest session
                var session = await _context.Session
                    .OrderByDescending(s => s.id)
                    .FirstOrDefaultAsync();

                if (session == null)
                {
                    _logger.LogWarning("No active session found.");
                    ViewBag.Error = "No active session found. Please contact the administrator.";
                    return View();
                }

                // Store the current session in the HTTP context
                HttpContext.Session.SetString("currsession", session.name);

                // Fetch the user by username
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.username == username);

                if (user != null)
                {
                    // Check if the password is hashed
                    bool isPasswordHashed = user.password.StartsWith("$2a$") || user.password.StartsWith("$2b$") || user.password.StartsWith("$2y$");

                    if (!isPasswordHashed)
                    {
                        // If the password is not hashed, hash it and update the database
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                        user.password = hashedPassword;
                        _context.Users.Update(user);
                        await _context.SaveChangesAsync();

                        _logger.LogInformation($"Password for user {username} has been hashed and updated.");
                    }

                    // Verify the password using BCrypt
                    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.password);

                    if (isPasswordValid)
                    {
                        // Store session variables
                        HttpContext.Session.SetString("UserRole", user.role);
                        HttpContext.Session.SetInt32("userid", user.id);

                        _logger.LogInformation($"User {username} logged in successfully.");
                        return RedirectToAction("Dashboard", "Dashboard");
                    }
                }

                // If username is a number, check if it's a valid user ID
                if (int.TryParse(username, out int userId))
                {
                    var userById = await _context.Users
                        .FirstOrDefaultAsync(u => u.id == userId);

                    if (userById != null)
                    {
                        // Check if the password is hashed
                        bool isPasswordHashed = userById.password.StartsWith("$2a$") || userById.password.StartsWith("$2b$") || userById.password.StartsWith("$2y$");

                        if (!isPasswordHashed)
                        {
                            // If the password is not hashed, hash it and update the database
                            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                            userById.password = hashedPassword;
                            _context.Users.Update(userById);
                            await _context.SaveChangesAsync();

                            _logger.LogInformation($"Password for user with ID {userId} has been hashed and updated.");
                        }

                        // Verify the password using BCrypt
                        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, userById.password);

                        if (isPasswordValid)
                        {
                            // Store session variables
                            HttpContext.Session.SetString("UserRole", userById.role);
                            HttpContext.Session.SetInt32("userid", userById.id);

                            _logger.LogInformation($"User with ID {userId} logged in successfully.");
                            return RedirectToAction("Dashboard", "Dashboard");
                        }
                    }
                }

                // If no user is found or password is invalid, show an error message
                ViewBag.Error = "Invalid Username or Password";
                _logger.LogWarning($"Failed login attempt for username: {username}.");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login.");
                ViewBag.Error = "An error occurred while processing your request. Please try again.";
                return View();
            }
        }

        // Logout
        public IActionResult Logout()
        {
            try
            {
                // Clear all session data
                HttpContext.Session.Clear();
                _logger.LogInformation("User logged out successfully.");
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout.");
                TempData["ErrorMessage"] = "An error occurred while logging out. Please try again.";
                return RedirectToAction("Login");
            }
        }
    }
}