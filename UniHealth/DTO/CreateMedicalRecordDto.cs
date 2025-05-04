using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO
{
    public class CreateMedicalRecordDto
    {
        [Required(ErrorMessage = " مطلوب")]  
        public int UserId { get; set; }

        [Required(ErrorMessage = " مطلوب")]
        public int HasSickness { get; set; }

        public string ImageUrl { get; set; }
    }
}
