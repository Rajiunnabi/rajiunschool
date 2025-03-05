using System.Collections.Specialized;
using System.Diagnostics.Eventing.Reader;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using rajiunschool.data;
using rajiunschool.Models;

namespace rajiunschool.Controllers
{
    public class SessionController : Controller
    {

        private readonly UmanagementContext _context;

        public SessionController(UmanagementContext context)
        {
            _context = context;
        }
        //Make a Controller name AddNewSemester
        public IActionResult AddNewSession()
        {
            Console.WriteLine("Kire vai ki choltase");
            return View();
        }
        //Make a controller name AddNewSemester which will store value in daatabase session
        public IActionResult AddNewSessionOperation(string newsession)
        {
            string cursession = HttpContext.Session.GetString("currsession");
            int flag = 0;
            var department = _context.Department.ToList();
            foreach(var dept in department)
            {
                int subjectcount = _context.SubjectLists.Count(s=>s.dept==dept.name);
                var studentcount = _context.ProfileStudents.Where(s => s.dept == dept.name && s.running == 0).ToList();

                int ulflag = 0;
                foreach(var student in studentcount)
                {
                    int subject = _context.CurrentCourseMarks.Count(s=>s.studentid == student.profileid && s.session == cursession);
                    Console.WriteLine($"Student Result Count:  Student Count:{student.profileid}, {subject}, Subject Count: {subjectcount}");
                    if (subject != subjectcount)
                    {
                        Console.WriteLine("How");
                        ulflag = 1;
                        flag = 1;
                    }

                }
                if (ulflag==1) break;
              
               

            }
            Console.WriteLine(flag);
            if (flag==0)
            {
                Console.WriteLine("VAi ami ki asholei vitore");
                var currentstudent = _context.ProfileStudents.Where(s => s.running == 0).ToList();

                foreach (var student in currentstudent)
                {
                    var result = _context.CurrentCourseMarks.Where(s => s.studentid == student.profileid && s.session == cursession).ToList();
                    int failedcourse = _context.FailedCourseMarks.Count(s => s.studentid == student.profileid);
                    int count = result.Count(m => m.totalmarks < 40.0);
                    foreach (var mark in result)
                    {
                        if (mark.totalmarks < 40.0)
                        {
                            var failedCourse = new failedcoursemark();
                            failedCourse.studentid = student.profileid;
                            failedCourse.subjectid = mark.subjectid;
                            failedCourse.session = cursession;
                            failedCourse.teacherid = mark.teacherid;
                            failedCourse.final = mark.final;
                            failedCourse.quiz1 = mark.quiz1;
                            failedCourse.quiz2 = mark.quiz2;
                            failedCourse.quiz3 = mark.quiz3;
                            failedCourse.quiz4 = mark.quiz4;
                            failedCourse.attendance = mark.attendance;
                            failedCourse.totalmarks = mark.totalmarks;
                            _context.FailedCourseMarks.Add(failedCourse);

                        }
                    }

                    if (count + failedcourse <= 4)
                    {
                        if (student.semester != "4.2")
                        {
                            student.session = newsession;
                        }
                        else student.session = "NA";
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
                        var failedCourses = _context.FailedCourseMarks.Where(s => s.studentid == student.profileid && s.session == cursession).ToList();
                        _context.FailedCourseMarks.RemoveRange(failedCourses);
                    }
                }
                var employe = _context.ProfileEmployees.Where(s => s.running == 0).ToList();
                foreach(var employee in employe)
                {
                    employee.session = newsession;
                }

                _context.SaveChanges(); // Save all changes at once

                var newSession = new session();
                newSession.name = newsession;// Renamed class
                _context.Session.Add(newSession);
                _context.SaveChanges();
                HttpContext.Session.SetString("currsession", newsession);
                return RedirectToAction("Dashboard", "Dashboard");
            }
            else
            {
                ViewBag.Error = "Not All Students got their marks";
                return View("AddNewSession"); // Ensure this view exists
            }
        }

    }
}
