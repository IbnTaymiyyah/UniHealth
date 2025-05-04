using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO
{
    public class CreateDoctorDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int UniversityId { get; set; }
    }
}
