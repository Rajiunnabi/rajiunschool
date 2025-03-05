using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace rajiunschool.Models
{
    public class profilestudent
    {
        [Key]
        public int hudao { get; set; }
        public int profileid { get; set; }
        public string name { get; set; }

        public string dept { get; set; }

        public string semester { get; set; }

        public string admittedsemester { get; set; }
        public string labclearancestatus { get; set; }
        public string? ProfilePicture { get; set; }
        public string session { get; set; }
        public int running { get; set; }

    }
}
