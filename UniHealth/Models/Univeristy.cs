using System.ComponentModel.DataAnnotations;

namespace UniHealth.Models
{
    public class Univeristy
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        
    }
}
