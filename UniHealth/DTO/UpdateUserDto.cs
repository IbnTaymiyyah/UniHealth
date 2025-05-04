using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = " مطلوب")]
        [MaxLength(100, ErrorMessage = "يجب ألا يتجاوز الاسم 100 حرف")]
        public string FName { get; set; }

        [Required(ErrorMessage = " مطلوب")]
        [MaxLength(100, ErrorMessage = "يجب ألا يتجاوز الاسم 100 حرف")]
        public string LName { get; set; }

        [Required(ErrorMessage = " مطلوب")]
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        public string PhoneNumber { get; set; }


        public string ImageUrl { get; set; }
    }
}
