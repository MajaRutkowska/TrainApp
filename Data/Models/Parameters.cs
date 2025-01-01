using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainApp.Data.Models
{
    public class Parameters
    {
        [Key]
        public string Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        [Required]
        public float Weight { get; set; }
        [Required]
        public float Height { get; set; }
        [Required]
        public float Speed { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
