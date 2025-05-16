using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO.Auth
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = " مطلوب")]
        [MaxLength(100, ErrorMessage = "يجب ألا يتجاوز الاسم 100 حرف")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = " مطلوب")]
        [MaxLength(100, ErrorMessage = "يجب ألا يتجاوز الاسم 100 حرف")]
        public string LastName { get; set; }

        [Required(ErrorMessage = " مطلوب")]
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        public string PhoneNumber { get; set; }

        public string ProfileImageUrl { get; set; }
        public string ImageUrl { get; set; }
    }
}
