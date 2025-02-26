using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace rajiunschool.Models
{
    public class paymentviewforstudent
    {
        [Key]
        public int hudao { get; set; }
        public int studentid { get; set; }
        public int punishmentfee { get; set; }

        public int transictionid { get; set; }

        public int status { get; set; }

        public int tutionfee { get; set; }

        public int addmissionfee { get; set; }

        public int session { get; set; }

        public int transportationfee { get; set; }
    }
}
