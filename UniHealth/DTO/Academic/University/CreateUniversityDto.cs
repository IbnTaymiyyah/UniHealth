using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO.Academic.University
{
    public class CreateUniversityDto
    {
        [Required(ErrorMessage = " مطلوب")]
        [MaxLength(100, ErrorMessage = "يجب ألا يتجاوز الاسم 100 حرف")]
        public string Name { get; set; }
    }
}
