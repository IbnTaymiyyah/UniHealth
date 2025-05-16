using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO.Auth.Password
{
    public class ResetPasswordDTO
    {

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; }

        [Required(ErrorMessage = "كود التحقق مطلوب")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "كود التحقق يجب أن يكون 6 أرقام")]
        public string VerificationCode { get; set; }

        [Required(ErrorMessage = "كلمة المرور الجديدة مطلوبة"), DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون بين 6 و 100 حرف")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب"), DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقين")]
        public string ConfirmPassword { get; set; }
    }
}
