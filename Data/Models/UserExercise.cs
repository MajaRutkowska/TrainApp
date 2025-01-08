using System.ComponentModel.DataAnnotations.Schema;

namespace TrainApp.Data.Models
{
    public class UserExercise
    {
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        [ForeignKey("Exercise")]
        public string ExerciseId { get; set; }
        public bool IsCompleted { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Exercise Exercise { get; set; }
    }
}
