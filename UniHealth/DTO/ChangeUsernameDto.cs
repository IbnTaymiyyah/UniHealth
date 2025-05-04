using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO
{
    public class ChangeUsernameDto
    {
        [Required(ErrorMessage = " مطلوب")]
        [RegularExpression(@"^[a-zA-Zء-ي\s]+$", ErrorMessage = "يجب أن يحتوي الاسم على أحرف فقط")]
        public string NewUsername { get; set; }

        [Required(ErrorMessage = " مطلوب"), DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
    }
}
