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
                        joindate = DateTime.Now.ToString("MMMM yyyy"),
                        profileid = lastuser.id,
                        ProfilePicture = null


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
        public async Task<IActionResult> ViewPdfforStudent()
        {
            int? id= HttpContext.Session.GetInt32("userid");
            string cursession= HttpContext.Session.GetString("currsession");
            var studentinfo = await _context.PaymentViewForStudents
               .FirstOrDefaultAsync(s => s.studentid == id && s.session==cursession);
            return View("DownloadPdfforStudent",studentinfo);
        }
        public async Task<IActionResult> DownloadPdfforStudent(int id)
        {
            // Fetch the student information by ID
            var studentinfo = await _context.PaymentViewForStudents
                .FirstOrDefaultAsync(s => s.studentid == id);

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
            // Start with the base query
            var query = _context.PaymentViewForStudents
                    .AsQueryable()
                    .Where(s => s.transictionid==searchQuery);

            Console.WriteLine($"User Role:, User ID:{searchQuery}");


            // Get the first matching record
            var paymentRecord = query.FirstOrDefault();

            if (paymentRecord != null)
            {
                // Update the status to "Payment completed"
                Console.WriteLine($"User Role: {paymentRecord.transictionid}, User ID: {paymentRecord.status}");
                paymentRecord.status = "Payment completed";

                // Save the changes to the database
                _context.SaveChanges();
            }

            // Fetch the updated list of records (after saving changes)
            var studentinfo = query.ToList();

            // Pass the updated list to the view
            return View("BankerUpdatePayment", studentinfo);
        }




    }
}
