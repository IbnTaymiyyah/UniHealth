using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO
{
    public class UpdateStudentDetailDto
    {
        [Required]
        public int CollegeId { get; set; }

        [Required]
        public int UniversityId { get; set; }
    }
}
