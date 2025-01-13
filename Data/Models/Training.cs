using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainApp.Data.Models
{
    public class Training
    {
        [Key]
        public string TrainingId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime Date { get; set; }
        public string Color { get; set; }

        [Required]
        [ForeignKey("Team")]
        public string TeamId { get; set; }
        
        public virtual Team Team { get; set; }  
    }
}
