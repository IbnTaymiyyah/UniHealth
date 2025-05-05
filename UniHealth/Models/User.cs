using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniHealth.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = " مطلوب")]
        [MaxLength(100, ErrorMessage = "يجب ألا يتجاوز الاسم 100 حرف")]
        public string FName { get; set; }

        [Required(ErrorMessage = " مطلوب")]
        [MaxLength(100, ErrorMessage = "يجب ألا يتجاوز الاسم 100 حرف")]
        public string LName { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; }

        [Required(ErrorMessage = " مطلوب")]
        [MaxLength(20, ErrorMessage = "يجب ألا يتجاوز الاسم 20 رقم")]
        public string phoneNumber { get; set; }

        public string imageUrl { get; set; }

        [Required(ErrorMessage = " مطلوب")]
        [RegularExpression(@"^[a-zA-Zء-ي\s]+$", ErrorMessage = "يجب أن يحتوي الاسم على أحرف فقط")]
        public string userName { get; set; }

        [Required(ErrorMessage = " مطلوب"), DataType(DataType.Password)]
        public string password { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
