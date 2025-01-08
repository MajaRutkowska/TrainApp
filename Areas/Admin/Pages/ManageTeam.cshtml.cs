
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
        public List<ApplicationUser> Coaches { get; set; } = new List<ApplicationUser>();
        private TeamUser TeamCoach { get; set; }

        public void OnGet(string teamId)
        {
            Team = _context.Team.FirstOrDefault(t => t.TeamId == teamId);

            var coachRoleId = _context.Roles
                   .Where(r => r.Name == "Coach")
                   .Select(r => r.Id)
                   .FirstOrDefault();

            if (coachRoleId != null)
            {
                var teamCoaches = _context.TeamUsers
                    .Where(tu => tu.TeamId == teamId)
                    .Join(_context.UserRoles,
                          tu => tu.UserId,
                          ur => ur.UserId,
                          (tu, ur) => new { tu.UserId, ur.RoleId })
                    .Where(x => x.RoleId == coachRoleId)
                    .ToList();

                Coaches = teamCoaches
                    .Select(tc => _context.Users.FirstOrDefault(u => u.Id == tc.UserId))
                    .ToList();
            }
        }

     }
}
