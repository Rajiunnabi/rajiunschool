using Microsoft.AspNetCore.Mvc;
using rajiunschool.data;
using rajiunschool.Models;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace rajiunschool.Controllers
{
    public class SessionController : Controller
    {
        private readonly UmanagementContext _context;
        private readonly ILogger<SessionController> _logger;

        public SessionController(UmanagementContext context, ILogger<SessionController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Session/AddNewSession
        public IActionResult AddNewSession()
        {
            return View();
        }

        // POST: Session/AddNewSessionOperation
        public IActionResult AddNewSessionOperation(string newsession)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newsession))
                {
                    ViewBag.Error = "New session name is required.";
                    return View("AddNewSession");
                }

                string cursession = HttpContext.Session.GetString("currsession");
                if (string.IsNullOrEmpty(cursession))
                {
                    ViewBag.Error = "Current session not found in session.";
                    return View("AddNewSession");
                }

                int flag = 0;
                var departments = _context.Department.ToList();

                // Check if all students have their marks
                foreach (var dept in departments)
                {
                    var students = _context.ProfileStudents
                        .Where(s => s.dept == dept.name && s.running == 0)
                        .ToList();

                    foreach (var student in students)
                    {
                        int subjectCount = _context.CurrentCourseMarks
                            .Count(s => s.studentid == student.profileid && s.session == cursession);

                        int expectedSubjectCount = _context.SubjectLists
                            .Count(s => s.dept == dept.name && s.semester == student.semester);

                        if (subjectCount != expectedSubjectCount)
                        {
                            flag = 1;
                            break;
                        }
                    }

                    if (flag == 1) break;
                }

                if (flag == 0)
                {
                    // Process students for the new session
                    var currentStudents = _context.ProfileStudents
                        .Where(s => s.running == 0)
                        .ToList();

                    foreach (var student in currentStudents)
                    {
                        var results = _context.CurrentCourseMarks
                            .Where(s => s.studentid == student.profileid && s.session == cursession)
                            .ToList();

                        int failedCourseCount = _context.FailedCourseMarks
                            .Count(s => s.studentid == student.profileid);

                        int count = results.Count(m => m.totalmarks < 40.0);

                        // Add failed courses to the FailedCourseMarks table
                        foreach (var mark in results)
                        {
                            if (mark.totalmarks < 40.0)
                            {
                                var failedCourse = new failedcoursemark
                                {
                                    studentid = student.profileid,
                                    subjectid = mark.subjectid,
                                    session = cursession,
                                    teacherid = mark.teacherid,
                                    final = mark.final,
                                    quiz1 = mark.quiz1,
                                    quiz2 = mark.quiz2,
                                    quiz3 = mark.quiz3,
                                    quiz4 = mark.quiz4,
                                    attendance = mark.attendance,
                                    totalmarks = mark.totalmarks
                                };

                                _context.FailedCourseMarks.Add(failedCourse);
                            }
                        }

                        // Update student's semester and session
                        if (count + failedCourseCount <= 4)
                        {
                            if (student.semester != "4.2")
                            {
                                student.session = newsession;
                            }
                            else
                            {
                                student.session = "NA";
                            }

                            switch (student.semester)
                            {
                                case "1.1": student.semester = "1.2"; break;
                                case "1.2": student.semester = "2.1"; break;
                                case "2.1": student.semester = "2.2"; break;
                                case "2.2": student.semester = "3.1"; break;
                                case "3.1": student.semester = "3.2"; break;
                                case "3.2": student.semester = "4.1"; break;
                                case "4.1": student.semester = "4.2"; break;
                                case "4.2": student.running = 1; break;
                            }
                        }
                        else
                        {
                            // Remove failed courses if the student has too many failed courses
                            var failedCourses = _context.FailedCourseMarks
                                .Where(s => s.studentid == student.profileid && s.session == cursession)
                                .ToList();

                            _context.FailedCourseMarks.RemoveRange(failedCourses);
                        }
                    }

                    // Update employee sessions
                    var employees = _context.ProfileEmployees
                        .Where(s => s.running == 0)
                        .ToList();

                    foreach (var employee in employees)
                    {
                        employee.session = newsession;
                    }

                    // Save all changes to the database
                    _context.SaveChanges();

                    // Add the new session to the Session table
                    var newSession = new session
                    {
                        name = newsession
                    };

                    _context.Session.Add(newSession);
                    _context.SaveChanges();

                    // Update the current session in the HTTP context
                    HttpContext.Session.SetString("currsession", newsession);

                    TempData["SuccessMessage"] = "New session added successfully.";
                    return RedirectToAction("Dashboard", "Dashboard");
                }
                else
                {
                    ViewBag.Error = "Not all students have received their marks.";
                    return View("AddNewSession");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddNewSessionOperation action.");
                TempData["ErrorMessage"] = "An error occurred while adding the new session.";
                return View("AddNewSession");
            }
        }
    }
}