using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace rajiunschool.Models
{
    public class borrowbooks
    {
        [Key]
        public int hudao { get; set; }
        public int studentid { get; set; }
        public string borrowedtime { get; set; }

        public string takenbacktime { get; set; }

    }
}
