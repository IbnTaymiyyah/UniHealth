using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO.Administration.Doctor
{
    public class UpdateDoctorDTO
    {


        [Required(ErrorMessage = " مطلوب")]
        public int UniversityId { get; set; }
    }
}
