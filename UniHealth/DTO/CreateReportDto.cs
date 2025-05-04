using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO
{
    public class CreateReportDto
    {

        [Required(ErrorMessage = " مطلوب")]
        public int UserId { get; set; }

        [Required(ErrorMessage = " مطلوب")]
        [MaxLength(200, ErrorMessage = "يجب ألا يتجاوز الاسم 200 حرف")]
        public string Title { get; set; }

        
        [MaxLength(2000, ErrorMessage = "يجب ألا يتجاوز الاسم 2000 حرف")]
        public string Description { get; set; }


        [MaxLength(2000, ErrorMessage = "يجب ألا يتجاوز الاسم 2000 حرف")]
        public string Notes { get; set; }
    }
}
