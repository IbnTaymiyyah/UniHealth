using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO
{
    public class ChangeUsernameDto
    {
        [Required]
        public string NewUsername { get; set; }

        [Required, DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
    }
}
