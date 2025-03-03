using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace rajiunschool.Models
{
    public class teacherevaluation
    {
        [Key]
        public int hudao { get; set; }
        public int teacherid { get; set; }
        public int subjectid {  get; set; }
        public int studentid { get; set; }
        public string details { get; set; }

    }
}
