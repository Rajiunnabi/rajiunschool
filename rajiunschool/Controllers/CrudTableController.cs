
using System.Collections.Specialized;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using rajiunschool.data;
using rajiunschool.Models;

namespace rajiunschool.Controllers
{
    public class CrudTableController : Controller
    {

        private readonly UmanagementContext _context;
       // public string UserListnow = ViewData["UserListnow"] as string ?? "";
        public CrudTableController(UmanagementContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Delete(int id,String UserListnow)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(UserListnow, "User");
        }
        [HttpGet]
        public IActionResult AddUser()
        {
            var model = new Dropdownmodel();
            model.Options = new List<SelectListItem>();
            model.Options.Add(new SelectListItem { Value = "Teacher", Text = "Teacher" });
            model.Options.Add(new SelectListItem { Value = "Student", Text = "Student" });
            model.Options.Add(new SelectListItem { Value = "Banker", Text = "Banker" });
            model.Options.Add(new SelectListItem { Value = "Libarian", Text = "Libarian" });
            model.Options.Add(new SelectListItem { Value = "Others", Text = "Others" });
            return View(model);
        }

        [HttpPost]
        public IActionResult AddUser(string Username, string Dept,string Password,Dropdownmodel model)
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Dept) || string.IsNullOrWhiteSpace(Password))
            {
                ViewBag.Error = "All fields are required.";
                return View();
            }
            var newUser = new users();
            newUser.username = Username;
            newUser.role = model.SelectedOption;
            newUser.password = Password;
            newUser.password = Password;
            HttpContext.Session.SetString("UserListnow",newUser.role);
            _context.Users.Add(newUser);
            _context.SaveChanges();
            ViewData["UserListnow"] = newUser.role;
            ViewBag.Message = "User added successfully!";
            return RedirectToAction(newUser.role, "User");
        }
    }
}
