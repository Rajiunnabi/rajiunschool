
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rajiunschool.data;
using rajiunschool.Models;
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
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("UserList", "User");
        }
        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUser(string Username, string Dept, string Position)
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Dept) || string.IsNullOrWhiteSpace(Position))
            {
                ViewBag.Error = "All fields are required.";
                return View();
            }

            var newUser = new users();
            newUser.username = Username;
            newUser.role = Position;
            newUser.password = "123";

            _context.Users.Add(newUser);
            _context.SaveChanges();

            ViewBag.Message = "User added successfully!";
            return RedirectToAction("UserList", "User");
        }
    }
}
