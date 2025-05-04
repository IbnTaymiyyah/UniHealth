using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO
{
    public class CreateMedicalRecordDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int HasSickness { get; set; }

        public string ImageUrl { get; set; }
    }
}
