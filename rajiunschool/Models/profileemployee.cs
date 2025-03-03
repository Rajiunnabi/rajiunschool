using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace rajiunschool.Models
{
    public class profileemployee
    {
        [Key]
        public int hudao { get; set; }
        public int profileid { get; set; }
        public string name { get; set; }

        public string joindate { get; set; }

        public int? age { get; set; }

        public string? sex { get; set; }

        public string? bloodgroup { get; set; }
        public string? details { get; set; }
        public string dept { get; set; }
        public string? ProfilePicture { get; set; }
        public string session { get; set; }

    }
}
