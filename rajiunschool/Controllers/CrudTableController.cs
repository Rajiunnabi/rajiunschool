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
using System.Reflection.Metadata;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using Microsoft.AspNetCore.Identity;
using iText.StyledXmlParser.Node;

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

        public async Task<IActionResult> DeleteUser(int id)
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
                var session = HttpContext.Session.GetString("currsession");
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                var lastuser = await _context.Users
                   .OrderByDescending(s => s.id)
                   .FirstOrDefaultAsync();
                if (model.SelectedOption.Equals("Student"))
                {
                    var newProfile = new profilestudent
                    {
                        profileid = lastuser.id,
                        name = Username,
                        dept = Dept,
                        semester = "1.1",
                        admittedsemester = session,
                        labclearancestatus = "NO DUE",
                        ProfilePicture = null,
                        session=session

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
                        joindate = DateTime.Now.ToString("MMMM yyyy"),
                        profileid = lastuser.id,
                        ProfilePicture = null,
                        session=session
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
        public IActionResult SubjectList(string searchQuery)
        {
            var subjects = _context.SubjectLists.AsQueryable(); // Queryable for efficient filtering

            if (!string.IsNullOrEmpty(searchQuery))
            {
                if (int.TryParse(searchQuery, out int subjectId))
                {
                    // If the input is a number, search by Subject ID
                    subjects = subjects.Where(s => s.id == subjectId);
                }
                else
                {
                    // If the input is text, search by Subject Name
                    subjects = subjects.Where(s => s.subjectname.Contains(searchQuery));
                }
            }

            return View(subjects.ToList()); // Pass filtered or full list to the view
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

        public IActionResult SubjectRequest(string searchQuery)
        {
            var subjectrequests = _context.SubjectRequests.ToList();

            // Initialize the list to store the extended data
            List<subjectrequestextend> SubjectRequestExtend = new List<subjectrequestextend>();

            foreach (var subjectrequest in subjectrequests)
            {
                // Fetch the related subject data using the id
                var subject = _context.SubjectLists.FirstOrDefault(s => s.id == subjectrequest.subjectid);

                // Create a new object of subjectrequestextend
                var extendedSubjectRequest = new subjectrequestextend
                {
                    subject = subject, // Set the related subject
                    teacherid = subjectrequest.teacherid // Set the teacher ID from the subject request
                };

                // Add the extended data to the list
                SubjectRequestExtend.Add(extendedSubjectRequest);
            }

            // Apply search filtering
            if (!string.IsNullOrEmpty(searchQuery))
            {
                if (int.TryParse(searchQuery, out int subjectId))
                {
                    // If the input is a number, search by Subject ID
                    SubjectRequestExtend = SubjectRequestExtend
                        .Where(sr => sr.subject != null && sr.subject.id == subjectId)
                        .ToList();
                }
                else
                {
                    // If the input is text, search by Subject Name
                    SubjectRequestExtend = SubjectRequestExtend
                        .Where(sr => sr.subject != null && sr.subject.subjectname.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
            }

            return View(SubjectRequestExtend);
        }
        public async Task<IActionResult> DeleteSubjectRequest(int subjectid, int teacherid)
        {
            Console.WriteLine("VAi Matro method da dhuklam");
            var subjectRequest = _context.SubjectRequests
                .FirstOrDefault(s => s.subjectid == subjectid && s.teacherid == teacherid);
            Console.WriteLine(subjectRequest.teacherid);
            if (subjectRequest != null)
            {
                // Remove the entity
                _context.SubjectRequests.Remove(subjectRequest);
                // Save changes to the database
                _context.SaveChanges();
            }
            return RedirectToAction("SubjectRequest", "CrudTable");
        }
        public async Task<IActionResult> ViewPdfforStudent()
        {
            int? id= HttpContext.Session.GetInt32("userid");
            string cursession= HttpContext.Session.GetString("currsession");
            var studentinfo = await _context.PaymentViewForStudents
               .FirstOrDefaultAsync(s => s.studentid == id && s.session==cursession && s.status=="Due");
            return View("DownloadPdfforStudent",studentinfo);
        }
        public async Task<IActionResult> DownloadPdfforStudent(int id)
        {
            // Fetch the student information by ID
            string cursession = HttpContext.Session.GetString("currsession");
            var studentinfo = await _context.PaymentViewForStudents
                .FirstOrDefaultAsync(s => s.studentid == id && s.session==cursession && s.status=="Due");

            // Check if the student exists
            if (studentinfo == null)
            {
                return NotFound("Student not found."); // Return a 404 error if the student doesn't exist
            }

            // Create a memory stream to hold the PDF data
            using (MemoryStream stream = new MemoryStream())
            {
                // Initialize PdfWriter, PdfDocument, and Document
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                var document = new iText.Layout.Document(pdf); // Use fully qualified name to avoid ambiguity

                // Add a title to the PDF
                document.Add(new Paragraph("Student Semester Fee Details")
                    .SetFontSize(18)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(20)
                );
                // Add student details to the PDF
                document.Add(new Paragraph($"Student ID: {studentinfo.studentid}"));
                document.Add(new Paragraph($"Punishment Fee: {studentinfo.punishmentfee}"));
                document.Add(new Paragraph($"Tuition Fee: {studentinfo.tutionfee}"));
                document.Add(new Paragraph($"Admission Fee: {studentinfo.addmissionfee}"));
                document.Add(new Paragraph($"Transportation Fee: {studentinfo.transportationfee}"));
                document.Add(new Paragraph($"Session: {studentinfo.session}"));
                document.Add(new Paragraph($"Transicionid: {studentinfo.transictionid}"));

                // Add a separator for better readability
                document.Add(new Paragraph("--------------------------------------------------")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginTop(10)
                );

                // Close the document, PDF, and writer
                document.Close();
                pdf.Close();
                writer.Close();

                // Return the PDF as a file
                return File(stream.ToArray(), "application/pdf", $"StudentFees_{studentinfo.studentid}.pdf");
            }

        }

        public async Task<IActionResult> ViewPayment(string searchQuery)
        {
            string userrole = HttpContext.Session.GetString("UserRole");
            int? id = HttpContext.Session.GetInt32("userid");

            if (userrole.Equals("Student"))
            {
                // Start with the base query
                var query = _context.PaymentViewForStudents
                    .AsQueryable()
                    .Where(s => s.studentid == id); // Filter by specific ID

                // Apply additional filter if searchQuery is provided
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    query = query.Where(s => s.session.Contains(searchQuery));
                }

                // Execute the query and get all matching records as a list
                var studentinfo = query.ToList();

                // Pass the list to the view
                return View("ViewPaymentForStudent", studentinfo);
            }
            return View();

        }
        public async Task<IActionResult> BankerUpdatePayment(string searchQuery)
        {
            // Start with the base query
            // Start with the base query
            var query = _context.PaymentViewForStudents
                .AsQueryable();

            // Apply additional filter if searchQuery is provided
            if (!string.IsNullOrEmpty(searchQuery))
            {
                // Filter by transictionid
                query = query.Where(s => s.transictionid == searchQuery);
            }

            // Execute the query and get the matching records as a list
            var studentinfo = query.ToList();

            // Pass the list to the view
            return View("BankerUpdatePayment", studentinfo);
        }
        public async Task<IActionResult> PaymentStatusUpdate(string searchQuery)
        {
            
            // Filter by transictionid
                var query = _context.PaymentViewForStudents
            .Where(s => s.transictionid == searchQuery);

            // Get the first matching record
            var paymentRecord = query.FirstOrDefault();

            if (paymentRecord != null)
            {
                // Update the status to "Payment completed"
                paymentRecord.status = "Payment completed";

                // Save the changes to the database
                _context.SaveChanges();
            }
            return RedirectToAction("BankerUpdatePayment", "CrudTable");
            // Fetch the updated list of records (after saving changes)

            // Pass the updated list to the view
        }

        public IActionResult AddPaymentforStudent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPaymentforStudent(int admissionfee,int tutionfee,int transportationfee,String department)
        {
            try
            {
                 var users= _context.ProfileStudents
                .Where(u => u.dept == department)
                .ToList();
                foreach(var user in users)
                {
                        var payment = new paymentviewforstudent();
                        payment.session= HttpContext.Session.GetString("currsession");
                        payment.studentid = user.profileid;
                        payment.addmissionfee = admissionfee;
                        payment.tutionfee = tutionfee;
                        payment.transportationfee = transportationfee;
                        string randomstring = RandomStringGenerator.GenerateRandomString(12);
                        payment.transictionid = randomstring;
                        payment.status = "Due";
                        _context.PaymentViewForStudents.Add(payment);
                        _context.SaveChanges();
                }
                TempData["SuccessMessage"] = "Subject added successfully!";
                return RedirectToAction("Dashboard", "Dashboard");
            }
            catch
            {
                ViewBag.Error = "An error occurred while adding the subject.";
                return View();
            }
        }

        public async Task<IActionResult> AddSubjecttoTeacher(int subjectid,int teacherid)
        {
            // Filter by transictionid
           string cursession = HttpContext.Session.GetString("currsession");
          var teachersubjectadd = new teachercourseview();
            teachersubjectadd.subjectid = subjectid;
            teachersubjectadd.teacherid = teacherid;
            teachersubjectadd.session = cursession;
            _context.TeacherCourseViews.Add(teachersubjectadd);
            _context.SaveChanges();
            var subjectRequest = _context.SubjectRequests
    .FirstOrDefault(s => s.subjectid == subjectid && s.teacherid == teacherid);
            var subject = _context.SubjectLists
    .FirstOrDefault(s => s.id == subjectid);
            if (subjectRequest != null)
            {
                // Remove the entity
                _context.SubjectRequests.Remove(subjectRequest);

                // Save changes to the database
                _context.SaveChanges();
            }
            if (subject != null)
            {
                // Update the instructor field
                subject.instructor = teacherid;
                // Save changes to the database
                _context.SaveChanges();
            }
            //Create a list of subjectrequestextend class
            return RedirectToAction("SubjectRequest", "CrudTable");


        }

        public IActionResult TeacherEvaluation()
        {
            profilestudent student = _context.ProfileStudents.FirstOrDefault(s => s.profileid == HttpContext.Session.GetInt32("userid"));
            List<subjectlist> subjects = _context.SubjectLists.Where(s=> s.semester == student.semester && s.dept == student.dept && s.instructor!=null).ToList();
            List<teacherevaluation> teacherevaluations = _context.Teacherevaluations.Where(s => s.studentid == student.profileid).ToList();

            List<teacherevaluationlist> evaluationList = new List<teacherevaluationlist>();

            foreach (var subject in subjects)
            {
                int flag = 0;
                foreach (var evaluation in teacherevaluations)
                {
                    if (evaluation.subjectid == subject.id)
                    {
                        flag = 1;
                        break;
                    }

                }
                if (flag == 0)
                {
                    teacherevaluationlist teacherevaluationlist = new teacherevaluationlist();
                    teacherevaluationlist.subjectid = subject.id;
                    teacherevaluationlist.subjectname = subject.subjectname;
                    string teachername = _context.ProfileEmployees.FirstOrDefault(s => s.profileid == subject.instructor).name;
                    teacherevaluationlist.teachername = teachername;
                    teacherevaluationlist.teacherid = subject.instructor;
                    evaluationList.Add(teacherevaluationlist);
                    Console.WriteLine( $"{teacherevaluationlist.subjectname}");
                }
            }
            return View(evaluationList);
        }

        public IActionResult TeacherEvaluationform(int subjectid,string subjectname,string teachername,int teacherid)
        {
            teacherevaluationlist evaluation = new teacherevaluationlist();
            evaluation.subjectid = subjectid;
            evaluation.subjectname = subjectname;
            evaluation.teachername = teachername;
            evaluation.teacherid = teacherid;
            Console.WriteLine($"{evaluation.subjectid} , {evaluation.subjectname} , {evaluation.teacherid} , {evaluation.teachername}");
            return View(evaluation);
        }
        //Make a post method of TeacherEvaluationform
        public async Task<IActionResult> TeacherEvaluationformadd(int subjectid, string subjectname, string teachername, int teacherid, string details)
        {
            Console.WriteLine($"{subjectid} , {subjectname} , {teacherid} , {teachername}");

            try
            {
                var newEvaluation = new teacherevaluation
                {
                    studentid = (int)HttpContext.Session.GetInt32("userid"),
                    subjectid = subjectid,
                    teacherid = teacherid,
                    details = details
                };

                _context.Teacherevaluations.Add(newEvaluation);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Evaluation added successfully!";
                return RedirectToAction("TeacherEvaluation", "CrudTable");
            }
            catch
            {
                ViewBag.Error = "An error occurred while adding the evaluation.";
                Console.WriteLine("Error occurred during evaluation.");
                return RedirectToAction("TeacherEvaluation", "CrudTable");
            }
        }
        public IActionResult CourseReview()
        {
            var evaluations = _context.Teacherevaluations.Where(s => s.teacherid == HttpContext.Session.GetInt32("userid")).ToList();
            var teacherevaluationextend = new List<teacherevaluationextend>();
            foreach(var evaluation in evaluations)
            {
                var subject = _context.SubjectLists.FirstOrDefault(s => s.id == evaluation.subjectid);
                var evaluationextend = new teacherevaluationextend();
                evaluationextend.subjectname = subject.subjectname;
                evaluationextend.subjectid = subject.id;
                evaluationextend.details = evaluation.details;
                teacherevaluationextend.Add(evaluationextend);
            }
            return View(teacherevaluationextend);
        }

        // make a method name seeSubjectlistForTeacher
        public IActionResult SeeSubjectListForTeacher(int SearchQuery)
        {
            var teacherid = HttpContext.Session.GetInt32("userid");
            if (teacherid == null)
            {
                // Handle the case where teacherid is null (e.g., redirect to login)
                return RedirectToAction("Login", "Account");
            }

            var profile = _context.ProfileEmployees.FirstOrDefault(s => s.profileid == teacherid);
            if (profile == null)
            {
                // Handle the case where the profile is not found
                return NotFound("Profile not found.");
            }

            var subjects = _context.SubjectLists
                                   .Where(s => s.dept == profile.dept && s.instructor == null)
                                   .ToList();

            var subjectRequests = _context.SubjectRequests
                                          .Where(s => s.teacherid == teacherid)
                                          .ToList();

            var subjectAvailableToRequest = new List<subjectlist>();

            foreach (var subject in subjects)
            {
                var entity = subjectRequests.FirstOrDefault(s => s.subjectid == subject.id);
                if (entity == null)
                {
                    subjectAvailableToRequest.Add(subject);
                }
            }

            if (SearchQuery != 0)
            {
                subjectAvailableToRequest = subjectAvailableToRequest
                                            .Where(s => s.id == SearchQuery)
                                            .ToList();
            }

            return View(subjectAvailableToRequest);
        }

        public async Task<IActionResult> ApplyForSubject(int subjectid)
        {
            var subjectRequest = new subjectrequest();
            subjectRequest.subjectid = subjectid;
            subjectRequest.teacherid = (int)HttpContext.Session.GetInt32("userid");
            _context.SubjectRequests.Add(subjectRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction("SeeSubjectListForTeacher", "CrudTable");
        }
    }
}
