using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO
{
    public class CreateStudentDetailDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int CollegeId { get; set; }

        [Required]
        public int UniversityId { get; set; }
    }
}
