using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace rajiunschool.Models
{
    public class teachercourseview
    {
        [Key]
        public int hudao { get; set; }
        public int teacherid { get; set; }
        public int subjectid { get; set; }
        public string session { get; set; }
        public int classtaken { get; set; }
    }
}
