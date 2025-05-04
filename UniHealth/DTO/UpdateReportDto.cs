using System.ComponentModel.DataAnnotations;

namespace UniHealth.DTO
{
    public class UpdateReportDto
    {

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }
    }
}
