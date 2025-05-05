using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO.Auth.Password
{
    public class ForgetPasswordRequestDTO
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; }
    }
}
