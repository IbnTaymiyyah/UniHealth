using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO
{
    public class UpdateMedicalRecordDto
    {
        [Required]
        public int HasSickness { get; set; }

        public string ImageUrl { get; set; }
    }
}
