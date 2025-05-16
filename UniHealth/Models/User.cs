using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace UniHealth.Models
{
    public class User : IdentityUser
    {

        public virtual Doctor Doctor { get; set; }

        [Required(ErrorMessage = " مطلوب")]
        [MaxLength(100, ErrorMessage = "يجب ألا يتجاوز الاسم 100 حرف")]
        public string FName { get; set; }

        [Required(ErrorMessage = " مطلوب")]
        [MaxLength(100, ErrorMessage = "يجب ألا يتجاوز الاسم 100 حرف")]
        public string LName { get; set; }

        public string ProfileImageUrl { get; set; }
        public string imageUrl { get; set; } = string.Empty;
/*
        [Required(ErrorMessage = " مطلوب"), DataType(DataType.Password)]
        public string password { get; set; } = "lol707";
*/

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
