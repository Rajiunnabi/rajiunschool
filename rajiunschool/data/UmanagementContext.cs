using Microsoft.EntityFrameworkCore;
namespace rajiunschool.data
{
   

    public class UmanagementContext : DbContext
    {
        public UmanagementContext(DbContextOptions<UmanagementContext> options) : base(options) { }

    }

    public class users
    {
        public int id { get; set; }
        public string position { get; set; }
    }

    public class teachercourseview
    {
        public int id { get; set; }
        public string dept { get; set; }
        public string sem { get; set; }
        public int session { get; set; }
        public int classtaken { get; set; }
    }

    public class subjectrequest
    {
        public int id { get; set; }
        public int teacherid { get; set; }
    }

    public class subjectlist
    {
        public int id { get; set; }
        public string subjectname { get; set; }

        public string dept { get; set; }

        public string semester { get; set; }

        public int instructor { get; set; }

        public string details { get; set; }

        public int takaperclass { get; set; }

        public int session { get; set; }
    }

    public class studentnumbersheet
    {
        public int id { get; set; }
        public string semester { get; set; }

        public int subject1 { get; set; }

        public int subject2 { get; set; }

        public int subject3 { get; set; }

        public int subject4 { get; set; }

        public int subject5 { get; set; }

        public int subject6 { get; set; }

        public int subject7 { get; set; }

        public int subject8 { get; set; }

        public int mark1 { get; set; }

        public int mark2 { get; set; }

        public int mark3 { get; set; }

        public int mark4 { get; set; }

        public int mark5 { get; set; }

        public int mark6 { get; set; }

        public int mark7 { get; set; }

        public int mark8 { get; set; }

        public int session { get; set; }
    }

    public class profilestudent
    {
        public int id { get; set; }
        public string name { get; set; }

        public string dept { get; set; }

        public string semester { get; set; }

        public string admittedsemester { get; set; }

        public string labclearancestatus { get; set; }

        public int session { get; set; }
    }

    public class profileemployee
    {
        public int id { get; set; }
        public string name { get; set; }

        public string joindate { get; set; }

        public int age { get; set; }

        public string sex { get; set; }

        public string bloodgroup{ get; set; }

        public string dept { get; set; }

        public string details { get; set; }

        public int session { get; set; }
    }

    public class paymentviewforteacher
    {
        public int id { get; set; }
        public int subject1 { get; set; }

        public int subject2 { get; set; }

        public int subject3 { get; set; }

        public int subject4 { get; set; }

        public int bonusfee{ get; set; }

        public int session { get; set; }
    }

    public class paymentviewforstudent
    {
        public int id { get; set; }
        public int punishmentfee { get; set; }

        public int transictionid { get; set; }

        public int status { get; set; }

        public int tutionfee { get; set; }

        public int addmissionfee { get; set; }

        public int session { get; set; }

        public int transportationfee { get; set; }
    }

    public class paymentviewforothers
    {
        public int id { get; set; }
        public int amount { get; set; }

        public int bonus { get; set; }

        public int session { get; set; }
    }

    public class paymentinfo
    {
        public int id { get; set; }
        public string paymentdate { get; set; }

        public string paymentmethod { get; set; }

        public int session { get; set; }
    }

    public class failedcoursemark
    {
        public int id { get; set; }
        public int ct { get; set; }

        public int attendance { get; set; }

        public int final { get; set; }
    }

    public class currentcoursemark
    {
        public int id { get; set; }
        public int ctmark { get; set; }

        public int attendance { get; set; }

        public int final { get; set; }
    }

    public class borrowbooks
    {
        public int id { get; set; }
        public string borrowedtime { get; set; }

        public string takenbacktime { get; set; }

    }

    public class booklist
    {
        public string name { get; set; }
        public string author { get; set; }

        public int availablepiece { get; set; }

        public string location { get; set; }

        public string details { get; set; }

    }

}
