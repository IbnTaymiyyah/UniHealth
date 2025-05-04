using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO
{
    public class UpdateCollegeDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
