using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniHealth.Models
{
    public class MedicalRecord
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        [MaxLength(100)]
        public int hasSickness { get; set; }

        
        [MaxLength(100)]
        public DateTime? createdAt { get; set; }


        public string imageUrl { get; set; }

       

        
    }
}
