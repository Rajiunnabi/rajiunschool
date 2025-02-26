using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace rajiunschool.Models
{
    public class paymentviewforothers
    {
        [Key]
        public int hudao { get; set; }
        public int employeeid { get; set; }
        public int amount { get; set; }

        public int bonus { get; set; }

        public int session { get; set; }
    }
}
