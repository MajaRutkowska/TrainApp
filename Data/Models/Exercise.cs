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
        public string CreatedBy { get; set; }
        public DateTime EndDate { get; set; }  
       
        public string? TeamId { get; set; }

        public virtual Team? Team { get; set; }
       
        public ICollection<ApplicationUser> User { get; set; } = new List<ApplicationUser>();

        public virtual ICollection<UserExercise> UserExercise { get; set; } = new List<UserExercise>();

        public virtual ICollection<Material> Material { get; set; } = new List<Material>();
    }
}
