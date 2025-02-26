using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace rajiunschool.Models
{
    public class paymentviewforteacher
    {
        [Key]
        public int hudao { get; set; }
        public int teacherid { get; set; }
        public int subject1 { get; set; }

        public int subject2 { get; set; }

        public int subject3 { get; set; }

        public int subject4 { get; set; }

        public int bonusfee { get; set; }

        public int session { get; set; }
    }
}
