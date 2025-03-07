using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using rajiunschool.data;
using rajiunschool.Models;
using System.Threading.Tasks;
using System.Linq;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Extensions.Logging;
using System.Collections.Immutable;
using iText.Svg.Renderers.Impl;
using iText.Commons.Actions.Contexts;
using Org.BouncyCastle.Crypto.Generators;
using BCrypt.Net;

namespace rajiunschool.Controllers
{
    public class CrudTableController : Controller
    {
        private readonly UmanagementContext _context;
        private readonly ILogger<CrudTableController> _logger;

        public CrudTableController(UmanagementContext context, ILogger<CrudTableController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {id} not found.");
                    return NotFound();
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(user.role, "User");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user.");
                TempData["ErrorMessage"] = "An error occurred while deleting the user.";
                return RedirectToAction("Index");
            }
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
                return View(model);
            }

            // Hash the password using bcrypt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);

            var newUser = new users
            {
                username = Username,
                role = model.SelectedOption,
                password = hashedPassword,
                running = 0// Store the hashed password
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
                        ProfilePicture = "~/images/default-avatar.png", // Default image path
                        session = session,
                        running = 0
                    };
                _context.ProfileStudents.Add(newProfile);
            }
            else
            {
                var newProfile = new profileemployee
                {
                    name = Username,
                    dept = Dept,
                    joindate = DateTime.Now.ToString("MMMM yyyy"),
                    profileid = lastuser.id,
                    ProfilePicture = "~/images/default-avatar.png",
                    session = session,
                    running = 0
                };
                _context.ProfileEmployees.Add(newProfile);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "User added successfully!";
            return RedirectToAction(newUser.role, "User");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding user.");
            ViewBag.Error = "An error occurred while adding the user.";
            return View(model);
        }
    }

    public IActionResult SubjectList(string searchQuery)
        {
            try
            {
                var subjects = _context.SubjectLists.AsQueryable();

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    if (int.TryParse(searchQuery, out int subjectId))
                    {
                        subjects = subjects.Where(s => s.id == subjectId);
                    }
                    else
                    {
                        subjects = subjects.Where(s => s.subjectname.Contains(searchQuery));
                    }
                }

                return View(subjects.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving subject list.");
                TempData["ErrorMessage"] = "An error occurred while retrieving the subject list.";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> DeleteSubject(int id)
        {
            try
            {
                var subjectlist = await _context.SubjectLists.FindAsync(id);
                var teachrCourseView = _context.TeacherCourseViews
                    .FirstOrDefault(s => s.subjectid == id);
                if (subjectlist == null)
                {
                    _logger.LogWarning($"Subject with ID {id} not found.");
                    return NotFound();
                }

                _context.SubjectLists.Remove(subjectlist);
                _context.TeacherCourseViews.Remove(teachrCourseView);
                await _context.SaveChangesAsync();
                return RedirectToAction("SubjectList", "CrudTable");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting subject.");
                TempData["ErrorMessage"] = "An error occurred while deleting the subject.";
                return RedirectToAction("SubjectList", "CrudTable");
            }
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

                var newSubject = new subjectlist
                {
                    dept = subject.dept,
                    semester = subject.semester,
                    subjectname = subject.subjectname,
                    takaperclass = subject.takaperclass,
                    instructor = null
                };

                _context.SubjectLists.Add(newSubject);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Subject added successfully!";
                return RedirectToAction("SubjectList", "CrudTable");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding subject.");
                ViewBag.Error = "An error occurred while adding the subject.";
                return View(subject);
            }
        }

        public IActionResult SubjectRequest(string searchQuery)
        {
            try
            {
                var subjectrequests = _context.SubjectRequests.ToList();
                var SubjectRequestExtend = new List<subjectrequestextend>();

                foreach (var subjectrequest in subjectrequests)
                {
                    var subject = _context.SubjectLists.FirstOrDefault(s => s.id == subjectrequest.subjectid);
                    if (subject != null)
                    {
                        SubjectRequestExtend.Add(new subjectrequestextend
                        {
                            subject = subject,
                            teacherid = subjectrequest.teacherid
                        });
                    }
                }

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    if (int.TryParse(searchQuery, out int subjectId))
                    {
                        SubjectRequestExtend = SubjectRequestExtend
                            .Where(sr => sr.subject != null && sr.subject.id == subjectId)
                            .ToList();
                    }
                    else
                    {
                        SubjectRequestExtend = SubjectRequestExtend
                            .Where(sr => sr.subject != null && sr.subject.subjectname.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                    }
                }

                return View(SubjectRequestExtend);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving subject requests.");
                TempData["ErrorMessage"] = "An error occurred while retrieving subject requests.";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> DeleteSubjectRequest(int subjectid, int teacherid)
        {
            try
            {
                var subjectRequest = _context.SubjectRequests
                    .FirstOrDefault(s => s.subjectid == subjectid && s.teacherid == teacherid);

                if (subjectRequest == null)
                {
                    _logger.LogWarning($"Subject request with subject ID {subjectid} and teacher ID {teacherid} not found.");
                    return NotFound();
                }

                _context.SubjectRequests.Remove(subjectRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction("SubjectRequest", "CrudTable");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting subject request.");
                TempData["ErrorMessage"] = "An error occurred while deleting the subject request.";
                return RedirectToAction("SubjectRequest", "CrudTable");
            }
        }

        public async Task<IActionResult> ViewPdfforStudent(string transictionid)
        {
            try
            {

                if (string.IsNullOrEmpty(transictionid))
                {
                    _logger.LogWarning("Transictionid is missing");
                    return NotFound("MIssing Transictionid");
                }

                var studentinfo = await _context.PaymentViewForStudents
                    .FirstOrDefaultAsync(s => s.transictionid==transictionid);

                if (studentinfo == null)
                {
                    _logger.LogWarning("No student found with this transictionid");
                    return NotFound("No student found with this transictionid");
                }

                return View("DownloadPdfforStudent", studentinfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating PDF for student.");
                TempData["ErrorMessage"] = "An error occurred while generating the PDF.";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> DownloadPdfforStudent(string transictionid)
        {
            try
            {
                var studentinfo = await _context.PaymentViewForStudents
                    .FirstOrDefaultAsync(s => s.transictionid==transictionid);

                if (studentinfo == null)
                {
                    _logger.LogWarning($"Student with Trasictionid {transictionid} not found.");
                    return NotFound("Student not found.");
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    PdfWriter writer = new PdfWriter(stream);
                    PdfDocument pdf = new PdfDocument(writer);
                    var document = new iText.Layout.Document(pdf);

                    document.Add(new Paragraph("Student Semester Fee Details")
                        .SetFontSize(18)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginBottom(20)
                    );

                    document.Add(new Paragraph($"Student ID: {studentinfo.studentid}"));
                    document.Add(new Paragraph($"Punishment Fee: {studentinfo.punishmentfee}"));
                    document.Add(new Paragraph($"Tuition Fee: {studentinfo.tutionfee}"));
                    document.Add(new Paragraph($"Admission Fee: {studentinfo.addmissionfee}"));
                    document.Add(new Paragraph($"Transportation Fee: {studentinfo.transportationfee}"));
                    document.Add(new Paragraph($"Session: {studentinfo.session}"));
                    document.Add(new Paragraph($"Transaction ID: {studentinfo.transictionid}"));

                    document.Close();
                    pdf.Close();
                    writer.Close();

                    return File(stream.ToArray(), "application/pdf", $"StudentFees_{studentinfo.studentid}.pdf");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating PDF for student.");
                TempData["ErrorMessage"] = "An error occurred while generating the PDF.";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> ViewPayment(string searchQuery)
        {
            try
            {
                string userrole = HttpContext.Session.GetString("UserRole");
                int? id = HttpContext.Session.GetInt32("userid");

                if (userrole.Equals("Student"))
                {
                    var query = _context.PaymentViewForStudents
                        .AsQueryable()
                        .Where(s => s.studentid == id);

                    if (!string.IsNullOrEmpty(searchQuery))
                    {
                        query = query.Where(s => s.session.Contains(searchQuery) || s.status == searchQuery);
                    }

                    var studentinfo = query.ToList();
                    return View("ViewPaymentForStudent", studentinfo);
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment details.");
                TempData["ErrorMessage"] = "An error occurred while retrieving payment details.";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> BankerUpdatePayment(string searchQuery)
        {
            try
            {
                var query = _context.PaymentViewForStudents.AsQueryable();

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    query = query.Where(s => s.transictionid == searchQuery);
                }

                var studentinfo = query.ToList();
                return View("BankerUpdatePayment", studentinfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment details for banker.");
                TempData["ErrorMessage"] = "An error occurred while retrieving payment details.";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> PaymentStatusUpdate(string searchQuery)
        {
            try
            {
                var paymentRecord = _context.PaymentViewForStudents
                    .FirstOrDefault(s => s.transictionid == searchQuery);

                if (paymentRecord == null)
                {
                    _logger.LogWarning($"Payment record with transaction ID {searchQuery} not found.");
                    return NotFound("Payment record not found.");
                }

                paymentRecord.status = "Payment completed";
                await _context.SaveChangesAsync();
                return RedirectToAction("BankerUpdatePayment", "CrudTable");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating payment status.");
                TempData["ErrorMessage"] = "An error occurred while updating the payment status.";
                return RedirectToAction("BankerUpdatePayment", "CrudTable");
            }
        }

        public IActionResult AddPaymentforStudent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPaymentforStudent(int admissionfee, int tutionfee, int transportationfee, string department)
        {
            try
            {
                var users = _context.ProfileStudents
                    .Where(u => u.dept == department)
                    .ToList();

                foreach (var user in users)
                {
                    var payment = new paymentviewforstudent
                    {
                        session = HttpContext.Session.GetString("currsession"),
                        studentid = user.profileid,
                        addmissionfee = admissionfee,
                        tutionfee = tutionfee,
                        transportationfee = transportationfee,
                        transictionid = RandomStringGenerator.GenerateRandomString(12),
                        status = "Due"
                    };
                    _context.PaymentViewForStudents.Add(payment);
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Payments added successfully!";
                return RedirectToAction("Dashboard", "Dashboard");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding payments for students.");
                ViewBag.Error = "An error occurred while adding payments.";
                return View();
            }
        }

        public async Task<IActionResult> AddSubjecttoTeacher(int subjectid, int teacherid)
        {
            try
            {
                string cursession = HttpContext.Session.GetString("currsession");
                var teachersubjectadd = new teachercourseview
                {
                    subjectid = subjectid,
                    teacherid = teacherid,
                    session = cursession
                };

                _context.TeacherCourseViews.Add(teachersubjectadd);
                await _context.SaveChangesAsync();

                var subjectRequest = _context.SubjectRequests
                    .FirstOrDefault(s => s.subjectid == subjectid && s.teacherid == teacherid);

                if (subjectRequest != null)
                {
                    _context.SubjectRequests.Remove(subjectRequest);
                    await _context.SaveChangesAsync();
                }

                var subject = _context.SubjectLists
                    .FirstOrDefault(s => s.id == subjectid);

                if (subject != null)
                {
                    subject.instructor = teacherid;
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("SubjectRequest", "CrudTable");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding subject to teacher.");
                TempData["ErrorMessage"] = "An error occurred while adding the subject to the teacher.";
                return RedirectToAction("SubjectRequest", "CrudTable");
            }
        }

        public IActionResult TeacherEvaluation()
        {
            try
            {
                var student = _context.ProfileStudents
                    .FirstOrDefault(s => s.profileid == HttpContext.Session.GetInt32("userid"));

                if (student == null)
                {
                    _logger.LogWarning("Student profile not found.");
                    return NotFound("Student profile not found.");
                }

                var subjects = _context.SubjectLists
                    .Where(s => s.semester == student.semester && s.dept == student.dept && s.instructor != null)
                    .ToList();

                var teacherevaluations = _context.Teacherevaluations
                    .Where(s => s.studentid == student.profileid)
                    .ToList();

                var evaluationList = new List<teacherevaluationlist>();

                foreach (var subject in subjects)
                {
                    if (!teacherevaluations.Any(e => e.subjectid == subject.id))
                    {
                        var teachername = _context.ProfileEmployees
                            .FirstOrDefault(s => s.profileid == subject.instructor)?.name;

                        evaluationList.Add(new teacherevaluationlist
                        {
                            subjectid = subject.id,
                            subjectname = subject.subjectname,
                            teachername = teachername,
                            teacherid = subject.instructor
                        });
                    }
                }

                return View(evaluationList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving teacher evaluations.");
                TempData["ErrorMessage"] = "An error occurred while retrieving teacher evaluations.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult TeacherEvaluationform(int subjectid, string subjectname, string teachername, int teacherid)
        {
            try
            {
                var evaluation = new teacherevaluationlist
                {
                    subjectid = subjectid,
                    subjectname = subjectname,
                    teachername = teachername,
                    teacherid = teacherid
                };

                return View(evaluation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading teacher evaluation form.");
                TempData["ErrorMessage"] = "An error occurred while loading the evaluation form.";
                return RedirectToAction("TeacherEvaluation", "CrudTable");
            }
        }

        [HttpPost]
        public async Task<IActionResult> TeacherEvaluationformadd(int subjectid, string subjectname, string teachername, int teacherid, string details)
        {
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding teacher evaluation.");
                TempData["ErrorMessage"] = "An error occurred while adding the evaluation.";
                return RedirectToAction("TeacherEvaluation", "CrudTable");
            }
        }
        //make a method named ViewEvaluation
      
        public async Task<IActionResult> ViewEvaluation()
        {
            try
            {
                var evaluationlist= _context.Teacherevaluations
                    .Where(s => s.teacherid == HttpContext.Session.GetInt32("userid"))
                    .ToList();
                var evaluationList = new List<teacherevaluationextend>();
                foreach(var evaluation in evaluationlist)
                {
                    var subject = _context.SubjectLists
                        .FirstOrDefault(s => s.id == evaluation.subjectid);
                    evaluationList.Add(new teacherevaluationextend
                    {
                        subjectid = subject.id,
                        subjectname = subject.subjectname,
                        details = evaluation.details
                    });
                    Console.WriteLine(evaluation.details);
                }
                return View(evaluationList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding teacher evaluation.");
                TempData["ErrorMessage"] = "An error occurred while adding the evaluation.";
                return RedirectToAction("Dashboard","Dashboard");
            }
        }
        public IActionResult CourseReview()
        {
            try
            {
                var evaluations = _context.Teacherevaluations
                    .Where(s => s.teacherid == HttpContext.Session.GetInt32("userid"))
                    .ToList();

                var teacherevaluationextend = new List<teacherevaluationextend>();

                foreach (var evaluation in evaluations)
                {
                    var subject = _context.SubjectLists
                        .FirstOrDefault(s => s.id == evaluation.subjectid);

                    if (subject != null)
                    {
                        teacherevaluationextend.Add(new teacherevaluationextend
                        {
                            subjectname = subject.subjectname,
                            subjectid = subject.id,
                            details = evaluation.details
                        });
                    }
                }

                return View(teacherevaluationextend);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving course reviews.");
                TempData["ErrorMessage"] = "An error occurred while retrieving course reviews.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult SeeSubjectListForTeacher(int SearchQuery)
        {
            try
            {
                var teacherid = HttpContext.Session.GetInt32("userid");

                if (teacherid == null)
                {
                    _logger.LogWarning("Teacher ID is missing.");
                    return RedirectToAction("Login", "Account");
                }

                var profile = _context.ProfileEmployees
                    .FirstOrDefault(s => s.profileid == teacherid);

                if (profile == null)
                {
                    _logger.LogWarning("Teacher profile not found.");
                    return NotFound("Profile not found.");
                }

                var subjects = _context.SubjectLists
                    .Where(s => s.dept == profile.dept && s.instructor == null)
                    .ToList();

                var subjectRequests = _context.SubjectRequests
                    .Where(s => s.teacherid == teacherid)
                    .ToList();

                var subjectAvailableToRequest = subjects
                    .Where(s => !subjectRequests.Any(sr => sr.subjectid == s.id))
                    .ToList();

                if (SearchQuery != 0)
                {
                    subjectAvailableToRequest = subjectAvailableToRequest
                        .Where(s => s.id == SearchQuery)
                        .ToList();
                }

                return View(subjectAvailableToRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving subject list for teacher.");
                TempData["ErrorMessage"] = "An error occurred while retrieving the subject list.";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> ApplyForSubject(int subjectid)
        {
            try
            {
                var subjectRequest = new subjectrequest
                {
                    subjectid = subjectid,
                    teacherid = (int)HttpContext.Session.GetInt32("userid")
                };

                _context.SubjectRequests.Add(subjectRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction("SeeSubjectListForTeacher", "CrudTable");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying for subject.");
                TempData["ErrorMessage"] = "An error occurred while applying for the subject.";
                return RedirectToAction("SeeSubjectListForTeacher", "CrudTable");
            }
        }
        //Create a method named AddDept
        public IActionResult AddDept()
        {
            Console.WriteLine("Hurray I am here");
            return View();
        }
        //Crate a method name AddDeptAdd and add value to the Department table in the database
        [HttpPost]
        public async Task<IActionResult> AddDeptAdd(string deptname)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(deptname))
                {
                    ViewBag.Error = "All fields are required.";
                    return View();
                }
                var newDept = new department
                {
                   name = deptname
                };
                _context.Department.Add(newDept);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Department added successfully!";
                return RedirectToAction("DepartmentList", "CrudTable");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding department.");
                ViewBag.Error = "An error occurred while adding the department.";
                return View("AddDept");
            }
        }

        //make a method which will show the list of department and the method name Will be DeptList
        public IActionResult DepartmentList()
        {
            try
            {
                var departments = _context.Department.ToList();
                return View(departments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving department list.");
                TempData["ErrorMessage"] = "An error occurred while retrieving the department list.";
                return RedirectToAction("Index");
            }
        }
        //Add a method name DeleteDept which will delete the department from the database and if there any dept in ProfileStudent or ProfileEmployee with the same name as that Department then it ill not delete tha department and will show a error message there exist user in that department
        public async Task<IActionResult> DeleteDept(int id)
        {
            try
            {
                var dept = await _context.Department.FindAsync(id);
                if (dept == null)
                {
                    _logger.LogWarning($"Department with ID {id} not found.");
                    return NotFound();
                }
                var profilestudent = _context.ProfileStudents
                    .FirstOrDefault(s => s.dept == dept.name);
                var profileemployee = _context.ProfileEmployees
                    .FirstOrDefault(s => s.dept == dept.name);
                if (profilestudent != null || profileemployee != null)
                {
                    TempData["ErrorMessage"] = "There are users in this department. Please delete the users first.";
                    return RedirectToAction("DepartmentList", "CrudTable");
                }
                _context.Department.Remove(dept);
                await _context.SaveChangesAsync();
                return RedirectToAction("DepartmentList", "CrudTable");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting department.");
                TempData["ErrorMessage"] = "An error occurred while deleting the department.";
                return RedirectToAction("DepartmentList", "CrudTable");
            }
        }
    }

}