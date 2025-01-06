
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TrainApp.Data.Models;
using TrainApp.Data;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace TrainApp.Areas.Admin.Pages
{
    [Authorize(Roles ="Admin")]
    public class ManageTeamModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        
        public ManageTeamModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Team Team { get; set; }
        public ApplicationUser Coach {  get; set; }
        private TeamUser TeamCoach { get; set; }

        public void OnGet(string teamId)
        {
            Team = _context.Team.FirstOrDefault(t => t.TeamId == teamId);
            TeamCoach = _context.TeamUsers.FirstOrDefault(c => c.TeamId == teamId);
            if (TeamCoach != null)
            {
                Coach = _context.Users.FirstOrDefault(u => u.Id == TeamCoach.UserId);
            }
        }

     }
}
