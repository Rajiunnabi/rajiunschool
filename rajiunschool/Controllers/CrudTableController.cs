using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using rajiunschool.data;
using rajiunschool.Models;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Collections.Immutable;

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
            return RedirectToAction(user.role, "User");
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
                    new SelectListItem { Value = "Libarian", Text = "Libarian" },
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
                    password = Password
                };
                var session = await _context.Session
                    .OrderByDescending(s => s.id)
                    .FirstOrDefaultAsync();
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                var lastuser = await _context.Users
                   .OrderByDescending(s => s.id)
                   .FirstOrDefaultAsync();

                Console.WriteLine($"User found: {lastuser.id}");
                Console.WriteLine($"User found: {session.name}");
                if (model.SelectedOption.Equals("Student"))
                {
                    var newProfile = new profilestudent
                    {
                        profileid = lastuser.id,
                        name = Username,
                        dept = Dept,
                        semester = "1.1",
                        admittedsemester = session.name,
                        labclearancestatus = "NO DUE",
                        ProfilePicture = null

                    };
                    _context.ProfileStudents.Add(newProfile);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    var newProfile = new profileemployee
                    {
                        name = Username,
                        dept = Dept,
                        joindate=DateTime.Now.ToString("MMMM yyyy"),
                        profileid=lastuser.id


                    };
                    _context.ProfileEmployees.Add(newProfile);
                    await _context.SaveChangesAsync();


                }

                ViewBag.Message = "User added successfully!";
                return RedirectToAction(newUser.role, "User");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred: " + ex.Message;
                return View();
            }
        }
        public IActionResult SubjectList()
        {
            var subjectlist = _context.SubjectLists.ToList();

            return View(subjectlist);
        }


        public async Task<IActionResult> DeleteSubject(int id)
        {
            var subjectlist = await _context.SubjectLists.FindAsync(id);
            if (subjectlist == null)
            {
                return NotFound();
            }

            _context.SubjectLists.Remove(subjectlist);
            await _context.SaveChangesAsync();
            return RedirectToAction("SubjectList", "CrudTable");
        }
        public IActionResult AddSubject()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSubject(subjectlist subject)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(subject.dept) ||
                    string.IsNullOrWhiteSpace(subject.semester) ||
                    string.IsNullOrWhiteSpace(subject.subjectname))
                {
                    ViewBag.Error = "All fields are required.";
                    return View(subject);
                }
                Console.WriteLine("Hello from ASP.NET Core! {subject.dept}");

                var newSubject = new subjectlist
                {
                    dept = subject.dept,
                    semester = subject.semester,
                    subjectname = subject.subjectname,
                    takaperclass = subject.takaperclass,
                    instructor = null // Nullable field
                };

                _context.SubjectLists.Add(newSubject);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Subject added successfully!";
                return RedirectToAction("SubjectList", "CrudTable");
            }
            catch
            {
                ViewBag.Error = "An error occurred while adding the subject.";
                return View(subject);
            }
        }

        public IActionResult SubjectRequest()
        {
            var subjectrequests = _context.SubjectRequests.ToList();

            // Initialize the list to store the extended data
            List<subjectrequestextend> SubjectRequestExtend = new List<subjectrequestextend>();

            // Create a list to store the related Subject data
            foreach (var subjectrequest in subjectrequests)
            {
                // Fetch the related subject data using the id
                var subject = _context.SubjectLists.FirstOrDefault(s => s.id == subjectrequest.subjectid);
                var profileteacher = _context.ProfileEmployees.FirstOrDefault(s => s.profileid == subjectrequest.teacherid);
                // Create a new object of subjectrequestextend
                var extendedSubjectRequest = new subjectrequestextend
                {
                    subject = subject,        // Set the related subject
                    teacherid = subjectrequest.teacherid,
                    profileteacher = profileteacher// Set the teacher id from the subjectrequest
                };

                // Add the extended data to the list
                SubjectRequestExtend.Add(extendedSubjectRequest);
            }

            // Return the view with the extended data
            return View(SubjectRequestExtend);
        }




    }
}
