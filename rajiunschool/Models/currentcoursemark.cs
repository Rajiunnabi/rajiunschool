using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using iText.Commons.Utils;

namespace rajiunschool.Models
{
    public class currentcoursemark
    {
        [Key]
        public int hudao { get; set; }
        public int studentid { get; set; }
        public int teacherid { get; set; }
        public int subjectid { get; set; }
        public int quiz1 { get; set; }
        public int quiz2 { get; set; }
        public int quiz3 { get; set; }
        public int quiz4 { get; set; }

        public int attendance { get; set; }

        public int final { get; set; }
        public int totalmarks { get; set; }
        public string session {  get; set; }
    }
}
