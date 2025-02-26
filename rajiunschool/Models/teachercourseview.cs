using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace rajiunschool.Models
{
    public class teachercourseview
    {
        [Key]
        public int hudao { get; set; }
        public int teacher { get; set; }
        public string dept { get; set; }
        public string sem { get; set; }
        public int session { get; set; }
        public int classtaken { get; set; }
    }
}
