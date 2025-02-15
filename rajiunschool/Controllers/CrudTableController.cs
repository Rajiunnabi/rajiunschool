using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using rajiunschool.data;
using rajiunschool.Models;
using System.Threading.Tasks;

namespace rajiunschool.Controllers
{
    public class CrudTableController : Controller
    {
        private readonly UmanagementContext _context;

        public CrudTableController(UmanagementContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Delete(int id, String UserListnow)
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
            var model = new Dropdownmodel
            {
                Options = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Teacher", Text = "Teacher" },
                    new SelectListItem { Value = "Student", Text = "Student" },
                    new SelectListItem { Value = "Banker", Text = "Banker" },
                    new SelectListItem { Value = "Librarian", Text = "Librarian" },
                    new SelectListItem { Value = "Others", Text = "Others" }
                }
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(string Username, string Dept, string Password, Dropdownmodel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Dept) || string.IsNullOrWhiteSpace(Password))
                {
                    ViewBag.Error = "All fields are required.";
                    return View();
                }

                var newUser = new users
                {
                    username = Username,
                    role = model.SelectedOption,
                    password = Password,
                    ProfilePicture = null 
                };

                HttpContext.Session.SetString("UserListnow", newUser.role);
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                ViewBag.Message = "User added successfully!";
                return RedirectToAction(newUser.role, "User");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred: " + ex.Message;
                return View();
            }
        }






    }
}
