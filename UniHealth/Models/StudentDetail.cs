using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniHealth.Models
{
    public class StudentDetail
    {
        public int Id { get; set; }


        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        
        [ForeignKey("Univeristy")]
        public int UniveristyId { get; set; }
        public virtual University Univeristy { get; set; }
        
        [ForeignKey("College")]
        public int CollegeId { get; set; }
        public virtual College College { get; set; }
    }
}
