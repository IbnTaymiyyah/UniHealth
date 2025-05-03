using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniHealth.Models
{
    public class StudentDetail
    {
        public int Id { get; set; }


        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        
        [ForeignKey("Univeristy")]
        public int UniveristyId { get; set; }
        public Univeristy Univeristy { get; set; }
        
        [ForeignKey("College")]
        public int CollegeId { get; set; }
        public College College { get; set; }
    }
}
