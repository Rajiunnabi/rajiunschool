using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rajiunschool.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
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
                    hudao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    studentid = table.Column<int>(type: "int", nullable: false),
                    borrowedtime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    takenbacktime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowBooks", x => x.hudao);
                });

            migrationBuilder.CreateTable(
                name: "CurrentCourseMarks",
                columns: table => new
                {
                    hudao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    studentid = table.Column<int>(type: "int", nullable: false),
                    teacherid = table.Column<int>(type: "int", nullable: false),
                    subjectid = table.Column<int>(type: "int", nullable: false),
                    quiz1 = table.Column<int>(type: "int", nullable: false),
                    quiz2 = table.Column<int>(type: "int", nullable: false),
                    quiz3 = table.Column<int>(type: "int", nullable: false),
                    quiz4 = table.Column<int>(type: "int", nullable: false),
                    attendance = table.Column<int>(type: "int", nullable: false),
                    final = table.Column<int>(type: "int", nullable: false),
                    totalmarks = table.Column<int>(type: "int", nullable: false),
                    session = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentCourseMarks", x => x.hudao);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "FailedCourseMarks",
                columns: table => new
                {
                    hudao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    studentid = table.Column<int>(type: "int", nullable: false),
                    subjectid = table.Column<int>(type: "int", nullable: false),
                    teacherid = table.Column<int>(type: "int", nullable: false),
                    quiz1 = table.Column<int>(type: "int", nullable: false),
                    quiz2 = table.Column<int>(type: "int", nullable: false),
                    quiz3 = table.Column<int>(type: "int", nullable: false),
                    quiz4 = table.Column<int>(type: "int", nullable: false),
                    attendance = table.Column<int>(type: "int", nullable: false),
                    final = table.Column<int>(type: "int", nullable: false),
                    totalmarks = table.Column<int>(type: "int", nullable: false),
                    session = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FailedCourseMarks", x => x.hudao);
                });

            migrationBuilder.CreateTable(
                name: "PaymentInfos",
                columns: table => new
                {
                    hudao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userid = table.Column<int>(type: "int", nullable: false),
                    paymentdate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    paymentmethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    session = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentInfos", x => x.hudao);
                });

            migrationBuilder.CreateTable(
                name: "PaymentViewForOthers",
                columns: table => new
                {
                    hudao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employeeid = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    bonus = table.Column<int>(type: "int", nullable: false),
                    session = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentViewForOthers", x => x.hudao);
                });

            migrationBuilder.CreateTable(
                name: "PaymentViewForStudents",
                columns: table => new
                {
                    hudao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    studentid = table.Column<int>(type: "int", nullable: false),
                    punishmentfee = table.Column<int>(type: "int", nullable: false),
                    transictionid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tutionfee = table.Column<int>(type: "int", nullable: false),
                    addmissionfee = table.Column<int>(type: "int", nullable: false),
                    session = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    transportationfee = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentViewForStudents", x => x.hudao);
                });

            migrationBuilder.CreateTable(
                name: "PaymentViewForTeachers",
                columns: table => new
                {
                    hudao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    teacherid = table.Column<int>(type: "int", nullable: false),
                    subject1 = table.Column<int>(type: "int", nullable: false),
                    subject2 = table.Column<int>(type: "int", nullable: false),
                    subject3 = table.Column<int>(type: "int", nullable: false),
                    subject4 = table.Column<int>(type: "int", nullable: false),
                    bonusfee = table.Column<int>(type: "int", nullable: false),
                    session = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentViewForTeachers", x => x.hudao);
                });

            migrationBuilder.CreateTable(
                name: "ProfileEmployees",
                columns: table => new
                {
                    hudao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    profileid = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    joindate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    age = table.Column<int>(type: "int", nullable: true),
                    sex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bloodgroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dept = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    session = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    running = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileEmployees", x => x.hudao);
                });

            migrationBuilder.CreateTable(
                name: "ProfileStudents",
                columns: table => new
                {
                    hudao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    profileid = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dept = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    semester = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    admittedsemester = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    labclearancestatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    session = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    running = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileStudents", x => x.hudao);
                });

            migrationBuilder.CreateTable(
                name: "Routine",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dept = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    semester = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routine", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Session",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Session", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "StudentNumberSheets",
                columns: table => new
                {
                    hudao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    studentid = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_StudentNumberSheets", x => x.hudao);
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
                    instructor = table.Column<int>(type: "int", nullable: true),
                    takaperclass = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectLists", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SubjectRequests",
                columns: table => new
                {
                    hudao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    subjectid = table.Column<int>(type: "int", nullable: false),
                    teacherid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectRequests", x => x.hudao);
                });

            migrationBuilder.CreateTable(
                name: "TeacherCourseViews",
                columns: table => new
                {
                    hudao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    teacherid = table.Column<int>(type: "int", nullable: false),
                    subjectid = table.Column<int>(type: "int", nullable: false),
                    session = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    classtaken = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherCourseViews", x => x.hudao);
                });

            migrationBuilder.CreateTable(
                name: "Teacherevaluations",
                columns: table => new
                {
                    hudao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    teacherid = table.Column<int>(type: "int", nullable: false),
                    subjectid = table.Column<int>(type: "int", nullable: false),
                    studentid = table.Column<int>(type: "int", nullable: false),
                    details = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacherevaluations", x => x.hudao);
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
                name: "Department");

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
                name: "Routine");

            migrationBuilder.DropTable(
                name: "Session");

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
