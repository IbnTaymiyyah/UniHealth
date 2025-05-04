using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO
{
    public class UpdateUniversityDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
