using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Chartypaltform.Models
{
    public class DonorEvent
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Donor")]
        public string DonorId { get; set; }
        public virtual Donor Donor { get; set; }

        [ForeignKey("Event")]
        public int EventId { get; set; }
        public virtual Event Event { get; set; }
    }
}
