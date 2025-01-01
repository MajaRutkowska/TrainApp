﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainApp.Data.Models
{
    public class Team
    {
        [Key]
        public string TeamId { get; set; }
        [Required]
        public string TeamName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public int CreationYear { get; set; }

        public virtual ICollection<TeamUser> TeamUser { get; set; } = new List<TeamUser>();
        public virtual ICollection<Training> Training { get; set; } = new List<Training>();

    }
}
