using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniHealth.Models
{
    public class Doctor
    {

        [Key]
        public int Id { get; set; }



        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        
        [ForeignKey("University")]
        public int UniversityId { get; set; }
        public virtual University University { get; set; }
    }
}
