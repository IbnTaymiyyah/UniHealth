using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO
{
    public class ChangePasswordDto
    {
        [Required , DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        
        [Required , DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }

    }
}
