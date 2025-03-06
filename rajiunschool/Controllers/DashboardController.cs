using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace rajiunschool.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ILogger<DashboardController> logger)
        {
            _logger = logger;
        }

        public IActionResult Dashboard()
        {
            try
            {
                var role = HttpContext.Session.GetString("UserRole");

                // Check if the user is authenticated
                if (string.IsNullOrEmpty(role))
                {
                    _logger.LogWarning("Unauthorized access attempt to Dashboard.");
                    return RedirectToAction("Login", "Auth");
                }

                // Redirect based on the user's role
                switch (role)
                {
                    case "Admin":
                        return RedirectToAction("AdminDashboard");
                    case "Student":
                        return RedirectToAction("StudentDashboard");
                    case "Teacher":
                        return RedirectToAction("TeacherDashboard");
                    case "Employee":
                        return RedirectToAction("EmployeeDashboard");
                    case "Banker":
                        return RedirectToAction("BankerDashboard");
                    default:
                        _logger.LogWarning($"Unknown role '{role}' detected.");
                        return RedirectToAction("Login", "Auth");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Dashboard action.");
                TempData["ErrorMessage"] = "An error occurred while processing your request. Please try again.";
                return RedirectToAction("Login", "Auth");
            }
        }

        public IActionResult AdminDashboard()
        {
            try
            {
                // Check if the user is authorized
                if (HttpContext.Session.GetString("UserRole") != "Admin")
                {
                    _logger.LogWarning("Unauthorized access attempt to AdminDashboard.");
                    return RedirectToAction("Login", "Auth");
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AdminDashboard action.");
                TempData["ErrorMessage"] = "An error occurred while loading the Admin Dashboard.";
                return RedirectToAction("Login", "Auth");
            }
        }

        public IActionResult StudentDashboard()
        {
            try
            {
                // Check if the user is authorized
                if (HttpContext.Session.GetString("UserRole") != "Student")
                {
                    _logger.LogWarning("Unauthorized access attempt to StudentDashboard.");
                    return RedirectToAction("Login", "Auth");
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in StudentDashboard action.");
                TempData["ErrorMessage"] = "An error occurred while loading the Student Dashboard.";
                return RedirectToAction("Login", "Auth");
            }
        }

        public IActionResult TeacherDashboard()
        {
            try
            {
                // Check if the user is authorized
                if (HttpContext.Session.GetString("UserRole") != "Teacher")
                {
                    _logger.LogWarning("Unauthorized access attempt to TeacherDashboard.");
                    return RedirectToAction("Login", "Auth");
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TeacherDashboard action.");
                TempData["ErrorMessage"] = "An error occurred while loading the Teacher Dashboard.";
                return RedirectToAction("Login", "Auth");
            }
        }

        public IActionResult EmployeeDashboard()
        {
            try
            {
                // Check if the user is authorized
                if (HttpContext.Session.GetString("UserRole") != "Employee")
                {
                    _logger.LogWarning("Unauthorized access attempt to EmployeeDashboard.");
                    return RedirectToAction("Login", "Auth");
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in EmployeeDashboard action.");
                TempData["ErrorMessage"] = "An error occurred while loading the Employee Dashboard.";
                return RedirectToAction("Login", "Auth");
            }
        }

        public IActionResult BankerDashboard()
        {
            try
            {
                // Check if the user is authorized
                if (HttpContext.Session.GetString("UserRole") != "Banker")
                {
                    _logger.LogWarning("Unauthorized access attempt to BankerDashboard.");
                    return RedirectToAction("Login", "Auth");
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in BankerDashboard action.");
                TempData["ErrorMessage"] = "An error occurred while loading the Banker Dashboard.";
                return RedirectToAction("Login", "Auth");
            }
        }
    }
}