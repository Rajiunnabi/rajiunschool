using System.ComponentModel.DataAnnotations;

namespace rajiunschool.Models
{
    public class TeacherRoutine
    {
        [Key]
        public int Id { get; set; }
        public int teacherprofileid { get; set; }
        public string TeacherUsername { get; set; }
        public string Department { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}