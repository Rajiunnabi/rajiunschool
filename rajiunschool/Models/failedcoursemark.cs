using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace rajiunschool.Models
{
    public class failedcoursemark
    {
        [Key]
       public int hudao {  get; set; }
        public int studentid { get; set; }
        public int ct { get; set; }

        public int attendance { get; set; }

        public int final { get; set; }
    }
}
