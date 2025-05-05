using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO.Auth.Password
{
    public class VerifyResetPasswordDTO
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; }

        [Required(ErrorMessage = "كود التحقق مطلوب")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "كود التحقق يجب أن يكون 6 أرقام")]
        public string VerificationCode { get; set; }
    }
}
