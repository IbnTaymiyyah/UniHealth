using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO.Auth.Password
{
    public class ChangePasswordDto
    {
        
        [Required(ErrorMessage = " مطلوب"), DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        
        [Required(ErrorMessage = " مطلوب"), DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "كلمة المرور غير متطابقة")]
        public string ConfirmNewPassword { get; set; }

    }
}
