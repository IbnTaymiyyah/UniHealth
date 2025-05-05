using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO.Administration.Doctor
{
    public class CreateDoctorDTO
    {
        [Required(ErrorMessage = " مطلوب")]
        public int UserId { get; set; }

        [Required(ErrorMessage = " مطلوب")]
        public int UniversityId { get; set; }
    }
}
