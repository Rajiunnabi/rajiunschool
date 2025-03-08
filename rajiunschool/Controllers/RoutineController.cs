using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using rajiunschool.Models;
using rajiunschool.data;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    public class RoutineController : Controller
    {
        private readonly UmanagementContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<RoutineController> _logger;

        public RoutineController(UmanagementContext context, IWebHostEnvironment hostingEnvironment, ILogger<RoutineController> logger)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }
        // GET: Routine/ManageTeacherRoutine
        public IActionResult ManageTeacherRoutine()
        {
            try
            {
                var routines = _context.TeacherRoutine.ToList();
                return View(routines);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ManageTeacherRoutine action.");
                TempData["ErrorMessage"] = "An error occurred while retrieving teacher routines.";
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Routine/UploadTeacherRoutine
        public IActionResult UploadTeacherRoutine()
        {
            return View();
        }

        // POST: Routine/UploadTeacherRoutine
        [HttpPost]
        public async Task<IActionResult> UploadTeacherRoutineAdd(int teacherid, string teacherUsername, string department, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    ViewBag.Error = "Please select a file to upload.";
                    return View("UploadTeacherRoutine");
                }

                if (string.IsNullOrWhiteSpace(teacherUsername) || string.IsNullOrWhiteSpace(department))
                {
                    ViewBag.Error = "Teacher username and department are required.";
                    return View("UploadTeacherRoutine");
                }

                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "teacher_routines");
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

                var routine = new TeacherRoutine
                {
                    teacherprofileid = teacherid,
                    TeacherUsername = teacherUsername,
                    Department = department,
                    FileName = file.FileName,
                    FilePath = Path.Combine("teacher_routines", fileName)
                };

                _context.TeacherRoutine.Add(routine);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Teacher routine uploaded successfully.";
                return RedirectToAction(nameof(ManageTeacherRoutine));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UploadTeacherRoutineAdd action.");
                TempData["ErrorMessage"] = "An error occurred while uploading the teacher routine.";
                return View("UploadTeacherRoutine");
            }
        }
        // GET: Routine/ViewTeacherRoutine
        public IActionResult ViewTeacherRoutine()
        {
            try
            {
                var userid = HttpContext.Session.GetInt32("userid") ?? throw new Exception("User ID not found in session.");
                
                var routine = _context.TeacherRoutine.FirstOrDefault(r => r.teacherprofileid == userid);

                if (routine == null)
                {
                    ViewBag.Message = "No routine available for your department.";
                    return View();
                }

                return View(routine);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ViewTeacherRoutine action.");
                TempData["ErrorMessage"] = "An error occurred while retrieving the routine.";
                return RedirectToAction("Index", "Home");
            }
        }
        // GET: Routine/DeleteTeacherRoutine/5
        public async Task<IActionResult> DeleteTeacherRoutine(int id)
        {
            try
            {
                var routine = await _context.TeacherRoutine.FindAsync(id);
                if (routine == null)
                {
                    TempData["ErrorMessage"] = "Teacher routine not found.";
                    return RedirectToAction(nameof(ManageTeacherRoutine));
                }

                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, routine.FilePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                _context.TeacherRoutine.Remove(routine);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Teacher routine deleted successfully.";
                return RedirectToAction(nameof(ManageTeacherRoutine));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteTeacherRoutine action.");
                TempData["ErrorMessage"] = "An error occurred while deleting the teacher routine.";
                return RedirectToAction(nameof(ManageTeacherRoutine));
            }
        }

        // GET: Routine/ManageRoutine
        public IActionResult ManageRoutine()
        {
            try
            {
                var routines = _context.Routine.ToList();
                return View(routines);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ManageRoutine action.");
                TempData["ErrorMessage"] = "An error occurred while retrieving routines.";
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Routine/UploadRoutine
        public IActionResult UploadRoutine()
        {
            return View();
        }

        // POST: Routine/UploadRoutine
        [HttpPost]
        public async Task<IActionResult> UploadRoutineAdd(string dept, string semester, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    ViewBag.Error = "Please select a file to upload.";
                    return View("UploadRoutine");
                }

                if (string.IsNullOrWhiteSpace(dept) || string.IsNullOrWhiteSpace(semester))
                {
                    ViewBag.Error = "Department and semester are required.";
                    return View("UploadRoutine");
                }

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

                var routine = new routine
                {
                    dept = dept,
                    semester = semester,
                    FileName = file.FileName,
                    FilePath = Path.Combine("routines", fileName)
                };

                _context.Routine.Add(routine);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Routine uploaded successfully.";
                return RedirectToAction(nameof(ManageRoutine));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UploadRoutineAdd action.");
                TempData["ErrorMessage"] = "An error occurred while uploading the routine.";
                return View("UploadRoutine");
            }
        }

        // GET: Routine/DeleteRoutine/5
        public async Task<IActionResult> DeleteRoutine(int id)
        {
            try
            {
                var routine = await _context.Routine.FindAsync(id);
                if (routine == null)
                {
                    TempData["ErrorMessage"] = "Routine not found.";
                    return RedirectToAction(nameof(ManageRoutine));
                }

                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, routine.FilePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                _context.Routine.Remove(routine);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Routine deleted successfully.";
                return RedirectToAction(nameof(ManageRoutine));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteRoutine action.");
                TempData["ErrorMessage"] = "An error occurred while deleting the routine.";
                return RedirectToAction(nameof(ManageRoutine));
            }
        }

        public IActionResult ViewStudentRoutine()
        {
            try
            {
                var userid = HttpContext.Session.GetInt32("userid") ?? throw new Exception("User ID not found in session.");
                var profile = _context.ProfileStudents.FirstOrDefault(p => p.profileid == userid);

                if (profile == null)
                {
                    TempData["ErrorMessage"] = "Student profile not found.";
                    return RedirectToAction("Index", "Home");
                }

                var routine = _context.Routine.FirstOrDefault(s => s.dept == profile.dept && s.semester == profile.semester);

                if (routine == null)
                {
                    ViewBag.Message = "No routine available for your department and semester.";
                    return View();
                }

                return View(routine);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ViewStudentRoutine action.");
                TempData["ErrorMessage"] = "An error occurred while retrieving the routine.";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}