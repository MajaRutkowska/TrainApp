using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainApp.Data.Models
{
    public class Exercise
    {
        [Key]
        public string ExerciseId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        [Required]
        [ForeignKey("Team")]
        public string TeamId { get; set; }

        public virtual Team Team { get; set; }
       
        public ICollection<ApplicationUser> User { get; set; } = new List<ApplicationUser>();
    }
}
