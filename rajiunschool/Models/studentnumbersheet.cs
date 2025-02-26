using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace rajiunschool.Models
{
    public class studentnumbersheet
    {
        [Key]
        public int hudao { get; set; }
        public int studentid { get; set; }
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
}
