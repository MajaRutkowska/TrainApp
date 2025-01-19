using Microsoft.AspNetCore.Identity;

namespace TrainApp.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }    
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }

        public virtual Parameters Parameters { get; set; }

        public virtual ICollection<TeamUser> TeamUser { get; set; } = new List<TeamUser>();
        public virtual ICollection<Exercise> Exercise { get; set; } = new List<Exercise>();
        public virtual ICollection<UserExercise> UserExercise { get; set; } = new List<UserExercise>();
        public virtual ICollection<Notes> Notes { get; set; } = new List<Notes>();
    }
}
