using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO.Medical.MedicalRecord
{
    public class UpdateMedicalRecordDto
    {
        [Required(ErrorMessage = " مطلوب")]
        public int HasSickness { get; set; }

        public string ImageUrl { get; set; }
    }
}
