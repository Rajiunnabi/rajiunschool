using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using rajiunschool.data;
using rajiunschool.Models;
using Microsoft.Extensions.Logging;

namespace rajiunschool.Controllers
{
    public class ResultController : Controller
    {
        private readonly UmanagementContext _context;
        private readonly ILogger<ResultController> _logger;

        public ResultController(UmanagementContext context, ILogger<ResultController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult CourseView()
        {
            try
            {
                int userid = HttpContext.Session.GetInt32("userid") ?? throw new Exception("User ID not found in session.");
                var mysubject = _context.SubjectLists.Where(x => x.instructor == userid).ToList();
                return View(mysubject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CourseView action.");
                TempData["ErrorMessage"] = "An error occurred while retrieving your courses.";
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult CourseViewStudent(subjectlist subject)
        {
            try
            {
                HttpContext.Session.SetString("Subject", JsonConvert.SerializeObject(subject));
                var students = _context.ProfileStudents
                    .Where(x => x.semester == subject.semester && x.dept == subject.dept)
                    .ToList();
                return View(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CourseViewStudent action.");
                TempData["ErrorMessage"] = "An error occurred while retrieving students.";
                return RedirectToAction("CourseView");
            }
        }

        public IActionResult StudentAddMark(int profileid)
        {
            try
            {
                int userid = HttpContext.Session.GetInt32("userid") ?? throw new Exception("User ID not found in session.");
                var subjectJson = HttpContext.Session.GetString("Subject") ?? throw new Exception("Subject data not found in session.");
                var subjectlist = JsonConvert.DeserializeObject<subjectlist>(subjectJson);

                var studentmark = new currentcoursemark
                {
                    studentid = profileid,
                    session = HttpContext.Session.GetString("currsession"),
                    subjectid = subjectlist.id,
                    teacherid = userid
                };

                var coursemark = _context.CurrentCourseMarks
                    .FirstOrDefault(x => x.studentid == profileid && x.subjectid == subjectlist.id);

                if (coursemark != null)
                {
                    studentmark.quiz1 = coursemark.quiz1;
                    studentmark.quiz2 = coursemark.quiz2;
                    studentmark.quiz3 = coursemark.quiz3;
                    studentmark.quiz4 = coursemark.quiz4;
                    studentmark.attendance = coursemark.attendance;
                    studentmark.final = coursemark.final;
                    studentmark.totalmarks = coursemark.totalmarks;
                }

                return View(studentmark);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in StudentAddMark action.");
                TempData["ErrorMessage"] = "An error occurred while loading the student marks.";
                return RedirectToAction("CourseViewStudent");
            }
        }

        [HttpPost]
        public IActionResult StudentAddMarkAdd(currentcoursemark currentcoursemark)
        {
            try
            {
                int userid = HttpContext.Session.GetInt32("userid") ?? throw new Exception("User ID not found in session.");
                var subjectJson = HttpContext.Session.GetString("Subject") ?? throw new Exception("Subject data not found in session.");
                var subjectlist = JsonConvert.DeserializeObject<subjectlist>(subjectJson);

                // Step 1: Store quiz marks in an array and sort in descending order
                int[] quizMarks = { currentcoursemark.quiz1, currentcoursemark.quiz2, currentcoursemark.quiz3, currentcoursemark.quiz4 };
                quizMarks = quizMarks.OrderByDescending(q => q).ToArray();

                // Step 2: Sum the best 3 quizzes and compute average
                int bestThreeQuizTotal = quizMarks[0] + quizMarks[1] + quizMarks[2];
                int quizAverage = bestThreeQuizTotal / 3;

                // Step 3: Calculate total marks
                currentcoursemark.totalmarks = quizAverage + currentcoursemark.attendance + currentcoursemark.final;

                // Check if the record already exists
                var existingMark = _context.CurrentCourseMarks
                    .FirstOrDefault(x => x.studentid == currentcoursemark.studentid && x.subjectid == subjectlist.id);

                if (existingMark == null)
                {
                    _context.CurrentCourseMarks.Add(currentcoursemark);
                }
                else
                {
                    existingMark.quiz1 = currentcoursemark.quiz1;
                    existingMark.quiz2 = currentcoursemark.quiz2;
                    existingMark.quiz3 = currentcoursemark.quiz3;
                    existingMark.quiz4 = currentcoursemark.quiz4;
                    existingMark.attendance = currentcoursemark.attendance;
                    existingMark.final = currentcoursemark.final;
                    existingMark.totalmarks = currentcoursemark.totalmarks;
                }

                _context.SaveChanges();

                return RedirectToAction("CourseViewStudent", "Result", new
                {
                    id = subjectlist.id,
                    name = subjectlist.subjectname,
                    dept = subjectlist.dept,
                    semester = subjectlist.semester,
                    instructor = subjectlist.instructor,
                    takaperclass = subjectlist.takaperclass
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in StudentAddMarkAdd action.");
                TempData["ErrorMessage"] = "An error occurred while saving the student marks.";
                return RedirectToAction("StudentAddMark", new { profileid = currentcoursemark.studentid });
            }
        }

        public IActionResult ViewFailedStudent()
        {
            try
            {
                int userid = HttpContext.Session.GetInt32("userid") ?? throw new Exception("User ID not found in session.");
                var failedstudents = _context.FailedCourseMarks
                    .Where(x => x.teacherid == userid)
                    .ToList();
                return View(failedstudents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ViewFailedStudent action.");
                TempData["ErrorMessage"] = "An error occurred while retrieving failed students.";
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult FailedStudentAddMark(failedcoursemark failcourse)
        {
            try
            {
                return View(failcourse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in FailedStudentAddMark action.");
                TempData["ErrorMessage"] = "An error occurred while loading the failed student marks.";
                return RedirectToAction("ViewFailedStudent");
            }
        }

        [HttpPost]
        public IActionResult FailedStudentAddMarkAdd(failedcoursemark failedcoursemark)
        {
            try
            {
                // Step 1: Store quiz marks in an array and sort in descending order
                int[] quizMarks = { failedcoursemark.quiz1, failedcoursemark.quiz2, failedcoursemark.quiz3, failedcoursemark.quiz4 };
                quizMarks = quizMarks.OrderByDescending(q => q).ToArray();

                // Step 2: Sum the best 3 quizzes and compute average
                int bestThreeQuizTotal = quizMarks[0] + quizMarks[1] + quizMarks[2];
                int quizAverage = (int)Math.Ceiling((double)bestThreeQuizTotal / 3);

                // Step 3: Calculate total marks
                failedcoursemark.totalmarks = quizAverage + failedcoursemark.attendance + failedcoursemark.final;

                if (failedcoursemark.totalmarks >= 40.0)
                {
                    var failedCourse = _context.FailedCourseMarks
                        .FirstOrDefault(s => s.studentid == failedcoursemark.studentid && s.subjectid == failedcoursemark.subjectid);

                    if (failedCourse != null)
                    {
                        _context.FailedCourseMarks.Remove(failedCourse);
                        _context.SaveChanges();
                    }
                }

                return RedirectToAction("ViewFailedStudent", "Result");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in FailedStudentAddMarkAdd action.");
                TempData["ErrorMessage"] = "An error occurred while updating the failed student marks.";
                return RedirectToAction("FailedStudentAddMark", new { id = failedcoursemark.studentid });
            }
        }

        public IActionResult StudentResult(string sessionName = null)
        {
            try
            {
                int? studentId = HttpContext.Session.GetInt32("userid") ?? throw new Exception("User ID not found in session.");

                // Get available sessions
                var sessions = _context.CurrentCourseMarks
                    .Where(m => m.studentid == studentId)
                    .Select(m => m.session)
                    .Distinct()
                    .ToList();

                // Get results only if a session is selected
                var results = !string.IsNullOrEmpty(sessionName)
                    ? _context.CurrentCourseMarks
                        .Where(m => m.studentid == studentId && m.session == sessionName)
                        .ToList()
                    : new List<currentcoursemark>();

                ViewBag.Sessions = sessions;
                ViewBag.SelectedSession = sessionName;
                return View(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in StudentResult action.");
                TempData["ErrorMessage"] = "An error occurred while retrieving your results.";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}