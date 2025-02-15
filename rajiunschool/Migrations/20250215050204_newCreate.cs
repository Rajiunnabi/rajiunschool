using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rajiunschool.Migrations
{
    /// <inheritdoc />
    public partial class newCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Booklists",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    availablepiece = table.Column<int>(type: "int", nullable: false),
                    location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    details = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booklists", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "BorrowBooks",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    borrowedtime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    takenbacktime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowBooks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CurrentCourseMarks",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ctmark = table.Column<int>(type: "int", nullable: false),
                    attendance = table.Column<int>(type: "int", nullable: false),
                    final = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentCourseMarks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "FailedCourseMarks",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ct = table.Column<int>(type: "int", nullable: false),
                    attendance = table.Column<int>(type: "int", nullable: false),
                    final = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FailedCourseMarks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentInfos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    paymentdate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    paymentmethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    session = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentInfos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentViewForOthers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    amount = table.Column<int>(type: "int", nullable: false),
                    bonus = table.Column<int>(type: "int", nullable: false),
                    session = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentViewForOthers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentViewForStudents",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    punishmentfee = table.Column<int>(type: "int", nullable: false),
                    transictionid = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    tutionfee = table.Column<int>(type: "int", nullable: false),
                    addmissionfee = table.Column<int>(type: "int", nullable: false),
                    session = table.Column<int>(type: "int", nullable: false),
                    transportationfee = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentViewForStudents", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentViewForTeachers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    subject1 = table.Column<int>(type: "int", nullable: false),
                    subject2 = table.Column<int>(type: "int", nullable: false),
                    subject3 = table.Column<int>(type: "int", nullable: false),
                    subject4 = table.Column<int>(type: "int", nullable: false),
                    bonusfee = table.Column<int>(type: "int", nullable: false),
                    session = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentViewForTeachers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ProfileEmployees",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    joindate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    age = table.Column<int>(type: "int", nullable: false),
                    sex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bloodgroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dept = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    session = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileEmployees", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ProfileStudents",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dept = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    semester = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    admittedsemester = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    labclearancestatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    session = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileStudents", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "StudentNumberSheets",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    semester = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    subject1 = table.Column<int>(type: "int", nullable: false),
                    subject2 = table.Column<int>(type: "int", nullable: false),
                    subject3 = table.Column<int>(type: "int", nullable: false),
                    subject4 = table.Column<int>(type: "int", nullable: false),
                    subject5 = table.Column<int>(type: "int", nullable: false),
                    subject6 = table.Column<int>(type: "int", nullable: false),
                    subject7 = table.Column<int>(type: "int", nullable: false),
                    subject8 = table.Column<int>(type: "int", nullable: false),
                    mark1 = table.Column<int>(type: "int", nullable: false),
                    mark2 = table.Column<int>(type: "int", nullable: false),
                    mark3 = table.Column<int>(type: "int", nullable: false),
                    mark4 = table.Column<int>(type: "int", nullable: false),
                    mark5 = table.Column<int>(type: "int", nullable: false),
                    mark6 = table.Column<int>(type: "int", nullable: false),
                    mark7 = table.Column<int>(type: "int", nullable: false),
                    mark8 = table.Column<int>(type: "int", nullable: false),
                    session = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentNumberSheets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SubjectLists",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    subjectname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dept = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    semester = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    instructor = table.Column<int>(type: "int", nullable: false),
                    details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    takaperclass = table.Column<int>(type: "int", nullable: false),
                    session = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectLists", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SubjectRequests",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    teacherid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectRequests", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TeacherCourseViews",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dept = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    session = table.Column<int>(type: "int", nullable: false),
                    classtaken = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherCourseViews", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Teacherevaluations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dept = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    subjectid = table.Column<int>(type: "int", nullable: false),
                    details = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacherevaluations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booklists");

            migrationBuilder.DropTable(
                name: "BorrowBooks");

            migrationBuilder.DropTable(
                name: "CurrentCourseMarks");

            migrationBuilder.DropTable(
                name: "FailedCourseMarks");

            migrationBuilder.DropTable(
                name: "PaymentInfos");

            migrationBuilder.DropTable(
                name: "PaymentViewForOthers");

            migrationBuilder.DropTable(
                name: "PaymentViewForStudents");

            migrationBuilder.DropTable(
                name: "PaymentViewForTeachers");

            migrationBuilder.DropTable(
                name: "ProfileEmployees");

            migrationBuilder.DropTable(
                name: "ProfileStudents");

            migrationBuilder.DropTable(
                name: "StudentNumberSheets");

            migrationBuilder.DropTable(
                name: "SubjectLists");

            migrationBuilder.DropTable(
                name: "SubjectRequests");

            migrationBuilder.DropTable(
                name: "TeacherCourseViews");

            migrationBuilder.DropTable(
                name: "Teacherevaluations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
