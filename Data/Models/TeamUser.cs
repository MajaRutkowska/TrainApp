using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainApp.Data.Models
{
    public class TeamUser
    {
      
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

    
        [ForeignKey("Team")]
        public string TeamId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Team Team { get; set; }
    }
}
