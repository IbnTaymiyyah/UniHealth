using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO
{
    public class UpdateDoctorDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int UniversityId { get; set; }
    }
}
