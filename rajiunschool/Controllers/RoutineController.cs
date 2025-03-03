using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using rajiunschool.Models;
using rajiunschool.data;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace YourNamespace.Controllers
{
    public class RoutineController : Controller
    {
        private readonly UmanagementContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public RoutineController(UmanagementContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Routine/ManageRoutine
        public IActionResult ManageRoutine()
        {
            var routines = _context.Routine.ToList();
            return View(routines);
        }

        // GET: Routine/UploadRoutine
        public IActionResult UploadRoutine()
        {
            return View();
        }

        // POST: Routine/UploadRoutine
        [HttpPost]
        public async Task<IActionResult> UploadRoutineAdd(string dept,string semester, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "routines");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                routine routine = new routine();
                routine.dept = dept;
                routine.semester = semester;
                routine.FileName = file.FileName;
                routine.FilePath = Path.Combine("routines", fileName);

                _context.Routine.Add(routine);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(ManageRoutine));
            }

            return View("UploadRoutine");
        }

        // GET: Routine/DeleteRoutine/5
        public async Task<IActionResult> DeleteRoutine(int id)
        {
            var routine = await _context.Routine.FindAsync(id);
            if (routine != null)
            {
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, routine.FilePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                _context.Routine.Remove(routine);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManageRoutine));
        }
        public IActionResult ViewStudentRoutine()
        {
            // Get the current student's ID (you can use claims or session to get the logged-in user's ID)
            var userid= HttpContext.Session.GetInt32("userid").Value;
            var profile = _context.ProfileStudents.FirstOrDefault(p => p.profileid == userid);

            // Fetch the student's details from the database
            var routine = _context.Routine.FirstOrDefault(s => s.dept == profile.dept && s.semester==profile.semester );


            // Fetch the routine for the student's department and semester

            if (routine == null)
            {
                ViewBag.Message = "No routine available for your department and semester.";
                return View();
            }

            // Pass the routine to the view
            return View(routine);
        }
    }
}