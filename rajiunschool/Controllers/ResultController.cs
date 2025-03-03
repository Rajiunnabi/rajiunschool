using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            var subjectJson = HttpContext.Session.GetString("Subject");
            var subjectlist = JsonConvert.DeserializeObject<subjectlist>(subjectJson);
            var studentmark = new currentcoursemark();
            studentmark.studentid = profileid;
            studentmark.session = HttpContext.Session.GetString("currsession");
            studentmark.subjectid= subjectlist.id;
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
                string concatenatedId = string.Concat(profileid, "-", subjectlist.id);
                HttpContext.Session.SetInt32(concatenatedId, 1);

            }
            return View(studentmark);
        }
        public IActionResult StudentAddMarkAdd(currentcoursemark currentcoursemark)
        {
            // Validate the model
      
                var subjectJson = HttpContext.Session.GetString("Subject");
                var subjectlist = JsonConvert.DeserializeObject<subjectlist>(subjectJson);
                // Step 1: Store quiz marks in an array and sort in descending order
                int[] quizMarks = { currentcoursemark.quiz1, currentcoursemark.quiz2, currentcoursemark.quiz3, currentcoursemark.quiz4 };
                quizMarks = quizMarks.OrderByDescending(q => q).ToArray();

                // Step 2: Sum the best 3 quizzes and compute average
                int bestThreeQuizTotal = quizMarks[0] + quizMarks[1] + quizMarks[2];
                int quizAverage = (int)Math.Ceiling((double)bestThreeQuizTotal / 3);

                // Step 3: Calculate total marks
                currentcoursemark.totalmarks = quizAverage + currentcoursemark.attendance + currentcoursemark.final;
                string concatenatedId = string.Concat(currentcoursemark.studentid, "-", subjectlist.id);
            int flag = HttpContext.Session.GetInt32(concatenatedId) ?? 0;
            Console.WriteLine("balkjsdf ");
                Console.WriteLine(flag);
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

    }
}
