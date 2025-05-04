using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO
{
    public class CreateUserDTO
    {
        [Required]
        [MaxLength(100)]
        public string FName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LName { get; set; }
       

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }


        [Required]
        public int RoleId { get; set; }

        
        public string ImageUrl { get; set; }
    }
}
