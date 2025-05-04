using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniHealth.Models
{
    public class Doctor
    {

        [Key]
        public int Id { get; set; }


        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        
        [ForeignKey("University")]
        public int UniversityId { get; set; }
        public University University { get; set; }
    }
}
