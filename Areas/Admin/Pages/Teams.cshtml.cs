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

namespace TrainApp.Areas.Admin.Pages
{
    [Authorize(Roles ="Admin")]
    public class TeamsModel : PageModel
    {
       private readonly ApplicationDbContext _context;
        public TeamsModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<dynamic> Teams { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            Teams = await (from team in _context.Team
                           join teamUser in _context.TeamUsers on team.TeamId equals teamUser.TeamId into teamUsersGroup
                           from teamUser in teamUsersGroup.DefaultIfEmpty() 
                           join coach in _context.Users on teamUser.UserId equals coach.Id into coachGroup
                           from coach in coachGroup.DefaultIfEmpty() 
                           select new
                           {
                               Team = team,
                               CoachName = coach != null ? coach.Name + " " + coach.Surname : "Brak" // Jeśli brak trenera, wyświetl "Brak"
                           }).ToListAsync();
            return Page();
            
        }


        public async Task<IActionResult> OnPostAsync(string teamId)
        {
            return RedirectToPage("/ManageTeam", new {teamId});
        }

        }
}
