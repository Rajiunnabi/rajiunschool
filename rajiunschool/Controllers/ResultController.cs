using System.Collections.Specialized;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg;
using rajiunschool.data;
using rajiunschool.Models;

namespace rajiunschool.Controllers
{
    public class ResultController : Controller
    {
        private readonly UmanagementContext _context;

        public ResultController(UmanagementContext context)
        {
            _context = context;
        }
        public IActionResult CourseView()
        {
            int userid = HttpContext.Session.GetInt32("userid").Value;
            var mysubject= _context.SubjectLists.Where(x => x.instructor == userid).ToList();
            Console.WriteLine(userid);
            return View(mysubject);
        }
        public IActionResult CourseViewStudent(subjectlist subject)
        {
            HttpContext.Session.SetString("Subject", JsonConvert.SerializeObject(subject));
            var students = _context.ProfileStudents.Where(x => x.semester == subject.semester && x.dept==subject.dept).ToList();
            return View(students);
        }
        //make a controller name StudentAddMark
        public IActionResult StudentAddMark(int profileid)
        {
            int userid= HttpContext.Session.GetInt32("userid").Value;
            var subjectJson = HttpContext.Session.GetString("Subject");
            var subjectlist = JsonConvert.DeserializeObject<subjectlist>(subjectJson);
            var studentmark = new currentcoursemark();
            studentmark.studentid = profileid;
            studentmark.session = HttpContext.Session.GetString("currsession");
            studentmark.subjectid= subjectlist.id;
            studentmark.teacherid = userid;
            Console.WriteLine(profileid);
            var coursemark = _context.CurrentCourseMarks.Where(x=> x.studentid==profileid && x.subjectid==subjectlist.id).FirstOrDefault();
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
        //make a controller name FailStudentAddMark
       
        public IActionResult StudentAddMarkAdd(currentcoursemark currentcoursemark)
        {
            // Validate the model
               int userid = (int) HttpContext.Session.GetInt32("userid");
                var subjectJson = HttpContext.Session.GetString("Subject");
                var subjectlist = JsonConvert.DeserializeObject<subjectlist>(subjectJson);
                // Step 1: Store quiz marks in an array and sort in descending order
                int[] quizMarks = { currentcoursemark.quiz1, currentcoursemark.quiz2, currentcoursemark.quiz3, currentcoursemark.quiz4 };
                quizMarks = quizMarks.OrderByDescending(q => q).ToArray();

                // Step 2: Sum the best 3 quizzes and compute average
                int bestThreeQuizTotal = quizMarks[0] + quizMarks[1] + quizMarks[2];
                int quizAverage = bestThreeQuizTotal / 3;

                // Step 3: Calculate total marks
                currentcoursemark.totalmarks = quizAverage + currentcoursemark.attendance + currentcoursemark.final;
                var flag = _context.CurrentCourseMarks.Where(c=> c.studentid==currentcoursemark.studentid && c.subjectid == currentcoursemark.subjectid).Count();
           // Console.WriteLine(flag);
                // Save the currentcoursemark to the database
                if (flag==0)
                {
                    _context.CurrentCourseMarks.Add(currentcoursemark);
                    _context.SaveChanges();

                }
                else
                {
                    var coursemark = _context.CurrentCourseMarks.Where(x => x.studentid == currentcoursemark.studentid && x.subjectid == subjectlist.id).FirstOrDefault();
                    coursemark.quiz1 = currentcoursemark.quiz1;
                    coursemark.quiz2 = currentcoursemark.quiz2;
                    coursemark.quiz3 = currentcoursemark.quiz3;
                    coursemark.quiz4 = currentcoursemark.quiz4;
                    coursemark.attendance = currentcoursemark.attendance;
                    coursemark.final = currentcoursemark.final;
                    coursemark.totalmarks = currentcoursemark.totalmarks;
                     Console.WriteLine(userid);
                    _context.SaveChanges();
                }
               

                // Retrieve the subjectlist object from session
                if (string.IsNullOrEmpty(subjectJson))
                {
                    throw new Exception("Subject data not found in session.");
                }

                // Redirect to the CourseViewStudent action with subjectlist properties as route parameters
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

        public IActionResult ViewFailedStudent()
        {
            int userid = (int)HttpContext.Session.GetInt32("userid");

            // Get the list of failed students for the current teacher
            var failedstudents = _context.FailedCourseMarks
                .Where(x => x.teacherid == userid)
                .ToList();

            // Pass the list of profile students to the view
            return View(failedstudents);
        }
        public IActionResult FailedStudentAddMark(failedcoursemark failcourse)
        {

            return View(failcourse);
        }
        //make a method name FailedStudentAddMark
        public IActionResult FailedStudentAddMarkAdd(failedcoursemark failedcoursemark)
        {
            // Step 1: Store quiz marks in an array and sort in descending order
            int[] quizMarks = { failedcoursemark.quiz1, failedcoursemark.quiz2, failedcoursemark.quiz3, failedcoursemark.quiz4 };
            quizMarks = quizMarks.OrderByDescending(q => q).ToArray();
            Console.WriteLine($"Final: {failedcoursemark.final}, Student ID: {failedcoursemark.studentid}, Teacher ID: {failedcoursemark.teacherid}, Subject ID: {failedcoursemark.subjectid}");
            // Step 2: Sum the best 3 quizzes and compute average
            int bestThreeQuizTotal = quizMarks[0] + quizMarks[1] + quizMarks[2];
            int quizAverage = (int)Math.Ceiling((double)bestThreeQuizTotal / 3);

            // Step 3: Calculate total marks
            failedcoursemark.totalmarks = quizAverage + failedcoursemark.attendance + failedcoursemark.final;
            // Generate a unique key for the session

            // Check if the record already exists in the session

            // Save or update the failedcoursemark in the database
            if (failedcoursemark.totalmarks >= 40.0)
            {
                // Remove the failed course mark from the database
                var failedCourse = _context.FailedCourseMarks.Where(s => s.studentid == failedcoursemark.studentid && s.subjectid == failedcoursemark.subjectid).FirstOrDefault();
                var currentcoursemark = _context.CurrentCourseMarks.Where(x => x.studentid == failedcoursemark.studentid && x.subjectid == failedcoursemark.subjectid).FirstOrDefault();
                currentcoursemark.totalmarks = failedcoursemark.totalmarks;
                Console.WriteLine(currentcoursemark.totalmarks);
                _context.FailedCourseMarks.Remove(failedCourse);
                _context.SaveChanges();
            }
            return RedirectToAction("ViewFailedStudent","Result"); // Redirect to a relevant action after saving
        }
        // Show available sessions and results dynamically
        public IActionResult StudentResult(string sessionName = null)
        {
            int? studentId = HttpContext.Session.GetInt32("userid");
            if (studentId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

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
    }
}
