
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
using Microsoft.EntityFrameworkCore;

namespace TrainApp.Areas.Coach.Pages
{
    [Authorize(Roles ="Coach")]
    public class TeamDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TeamDetailsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Team Team { get; set; }
        public List<ApplicationUser> Players { get; set; } = new List<ApplicationUser>();

        public async Task<IActionResult> OnGetAsync(string teamId)
        {
            Team = _context.Team.FirstOrDefault(t => t.TeamId == teamId);

            Players = await (from user in _context.Users
                             join userRole in _context.UserRoles on user.Id equals userRole.UserId
                             join role in _context.Roles on userRole.RoleId equals role.Id
                             join teamUser in _context.TeamUsers on user.Id equals teamUser.UserId
                             where role.Name == "Player" && teamUser.TeamId == teamId
                             select user).ToListAsync();


            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string playerId, string teamId)
        {
            var teamUser = await _context.TeamUsers.FirstOrDefaultAsync(tu => tu.UserId == playerId && tu.TeamId == teamId);

            if (teamUser != null)
            {
                _context.TeamUsers.Remove(teamUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Coach/TeamDetails", new { teamid = teamId });
        }

    }
}
