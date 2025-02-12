using Microsoft.AspNetCore.Mvc;

namespace rajiunschool.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;
    using System.Linq;
    using rajiunschool.data;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    public class AuthController : Controller
    {
        private readonly UmanagementContext _context;

        public AuthController(UmanagementContext context)
        {
            _context = context;
        }

        // GET: Show Login Page
        public IActionResult Login()
        {
            return View();
        }

        // POST: Process Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.username == username && u.password == password);
            

            if (user != null)
            {
                // Store session variables
                HttpContext.Session.SetString("UserRole", user.role);
                HttpContext.Session.SetInt32("UserId", user.id);

                // Role-based redirection
                return RedirectToAction("Dashboard", "Dashboard");
            }
            else
            {
                int id;
                if (int.TryParse(username, out id))
                {
                    var user1 = _context.Users.FirstOrDefault(u => u.id == id && u.password == password);
                    if (user1 != null)
                    {
                        // Store session variables
                        HttpContext.Session.SetString("UserRole", user1.role);
                        HttpContext.Session.SetInt32("UserId", user1.id);

                        // Role-based redirection
                        return RedirectToAction("Dashboard", "Dashboard");
                    }
                }
                
            }

            ViewBag.Error = "Invalid Username or Password";
            return View();
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }

}
