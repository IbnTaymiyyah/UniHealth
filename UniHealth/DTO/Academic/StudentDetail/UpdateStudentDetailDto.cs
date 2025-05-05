using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO.Academic.StudentDetail
{
    public class UpdateStudentDetailDto
    {
        [Required(ErrorMessage = " مطلوب")]
        public int CollegeId { get; set; }

        [Required(ErrorMessage = " مطلوب")]
        public int UniversityId { get; set; }
    }
}
