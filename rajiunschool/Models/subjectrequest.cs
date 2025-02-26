using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace rajiunschool.Models
{
    public class subjectrequest
    {
        [Key]
        public int hudao { get; set; }
        public int subjectid { get; set; }
        public int teacherid { get; set; }
    }
}
