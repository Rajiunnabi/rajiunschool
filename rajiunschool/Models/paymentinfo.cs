using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace rajiunschool.Models
{
    public class paymentinfo
    {
        [Key]
        public int hudao { get; set; }
        public int userid { get; set; }
        public string paymentdate { get; set; }

        public string paymentmethod { get; set; }

        public int session { get; set; }
    }
}
