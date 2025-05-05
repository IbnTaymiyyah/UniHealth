using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO.Auth.Password
{
    public class ResetPasswordDTO
    {

        [Required(ErrorMessage = "كلمة المرور الجديدة مطلوبة"), DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون بين 6 و 100 حرف")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب"), DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقين")]
        public string ConfirmPassword { get; set; }
    }
}
