using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO.Auth
{
    public class CreateUserDTO
    {
        [Required(ErrorMessage = " مطلوب")]
        [MaxLength(100, ErrorMessage = "يجب ألا يتجاوز الاسم 100 حرف")]
        public string FName { get; set; }

        [Required(ErrorMessage = " مطلوب")]
        [MaxLength(100, ErrorMessage = "يجب ألا يتجاوز الاسم 100 حرف")]
        public string LName { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; }

        [Required(ErrorMessage = " مطلوب")]
        [MaxLength(20, ErrorMessage = "يجب ألا يتجاوز الاسم 20 رقم")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = " مطلوب")]
        [RegularExpression(@"^[a-zA-Zء-ي\s]+$", ErrorMessage = "يجب أن يحتوي الاسم على أحرف فقط")]
        public string Username { get; set; }

        [Required(ErrorMessage = " مطلوب"), DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessage = " مطلوب")]
        public int RoleId { get; set; }

      
        public string ImageUrl { get; set; }
    }
}
