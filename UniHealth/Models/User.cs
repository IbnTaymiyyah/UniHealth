using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniHealth.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LName { get; set; }

        [MaxLength(100)]
        public string phoneNumber { get; set; }

        [MaxLength(100)]
        public string imageUrl { get; set; }
        
        [MaxLength(100)]
        public string userName { get; set; }

        [MaxLength(100)]
        public string password { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
