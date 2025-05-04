using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO
{
    public class UpdateDoctorDTO
    {


        [Required]
        public int UniversityId { get; set; }
    }
}
