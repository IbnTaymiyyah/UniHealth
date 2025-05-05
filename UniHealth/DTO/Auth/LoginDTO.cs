using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO.Auth
{
    public class LoginDTO
    {
        [Required(ErrorMessage = " مطلوب")]
        [RegularExpression(@"^[a-zA-Zء-ي\s]+$", ErrorMessage = "يجب أن يحتوي الاسم على أحرف فقط")]
        public string Username { get; set; }

        [Required(ErrorMessage = " مطلوب"), DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

    }
}
