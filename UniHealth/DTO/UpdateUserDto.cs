using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO
{
    public class UpdateUserDto
    {
        [Required]
        [MaxLength(100)]
        public string FName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LName { get; set; }

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }


        public string ImageUrl { get; set; }
    }
}
