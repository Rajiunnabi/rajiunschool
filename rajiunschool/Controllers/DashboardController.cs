namespace rajiunschool.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;

    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(role))
            {
                return RedirectToAction("Login", "Auth");
            }

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
                default:
                    return RedirectToAction("Login", "Auth");
            }
        }

        public IActionResult AdminDashboard()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Login", "Auth");

            return View();
        }

        public IActionResult StudentDashboard()
        {
            if (HttpContext.Session.GetString("UserRole") != "Student")
                return RedirectToAction("Login", "Auth");

            return View();
        }

        public IActionResult TeacherDashboard()
        {
            if (HttpContext.Session.GetString("UserRole") != "Teacher")
                return RedirectToAction("Login", "Auth");

            return View();
        }

        public IActionResult EmployeeDashboard()
        {
            if (HttpContext.Session.GetString("UserRole") != "Employee")
                return RedirectToAction("Login", "Auth");

            return View();
        }
    }

}
