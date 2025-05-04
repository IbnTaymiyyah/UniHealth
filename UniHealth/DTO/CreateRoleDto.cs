using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO
{
    public class CreateRoleDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
