using Microsoft.EntityFrameworkCore;
using rajiunschool.Models;

namespace rajiunschool.data
{
    public class UmanagementContext : DbContext
    {
        public UmanagementContext(DbContextOptions<UmanagementContext> options) : base(options) { }

        public DbSet<booklist> Booklists { get; set; }
        public DbSet<borrowbooks> BorrowBooks { get; set; }
        public DbSet<currentcoursemark> CurrentCourseMarks { get; set; }
        public DbSet<failedcoursemark> FailedCourseMarks { get; set; }
        public DbSet<paymentinfo> PaymentInfos { get; set; }
        public DbSet<paymentviewforothers> PaymentViewForOthers { get; set; }
        public DbSet<paymentviewforstudent> PaymentViewForStudents { get; set; }
        public DbSet<paymentviewforteacher> PaymentViewForTeachers { get; set; }
        public DbSet<profileemployee> ProfileEmployees { get; set; }
        public DbSet<profilestudent> ProfileStudents { get; set; }
        public DbSet<studentnumbersheet> StudentNumberSheets { get; set; }
        public DbSet<subjectlist> SubjectLists { get; set; }
        public DbSet<subjectrequest> SubjectRequests { get; set; }
        public DbSet<teachercourseview> TeacherCourseViews { get; set; }
        public DbSet<users> Users { get; set; }
        public DbSet<teacherevaluation> Teacherevaluations { get; set; }
        public DbSet<session> Session { get; set; }

       
    }
}
