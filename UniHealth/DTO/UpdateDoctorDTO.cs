using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO
{
    public class UpdateDoctorDTO
    {


        [Required(ErrorMessage = " مطلوب")]
        public int UniversityId { get; set; }
    }
}
