using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniHealth.Models
{
    public class DoctorId
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DoctorUniversityId { get; set; } 

        [Required]
        public bool IsUsed { get; set; } = false; 

        
    }
}
