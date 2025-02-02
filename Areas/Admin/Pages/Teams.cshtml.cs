﻿using System;
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
        public async Task<IActionResult> OnGetAsync(string searchQuery)
        {
            var query = from team in _context.Team
                        join teamUser in _context.TeamUsers on team.TeamId equals teamUser.TeamId into teamUserGroup
                        from teamUser in teamUserGroup.DefaultIfEmpty() // LEFT JOIN z TeamUsers
                        join userRole in _context.UserRoles on teamUser.UserId equals userRole.UserId into userRoleGroup
                        from userRole in userRoleGroup.DefaultIfEmpty() // LEFT JOIN z UserRoles
                        join role in _context.Roles on userRole.RoleId equals role.Id into roleGroup
                        from role in roleGroup.DefaultIfEmpty() // LEFT JOIN z Roles
                        join coach in _context.Users on teamUser.UserId equals coach.Id into coachGroup
                        from coach in coachGroup.DefaultIfEmpty() // LEFT JOIN z Users
                        where role == null || role.Name == "Coach" // Filtr na trenerów lub brak roli
                        group coach by team into teamGroup
                        select new
                        {
                            Team = teamGroup.Key,
                            CoachName = teamGroup.Any(c => c != null)
                                ? string.Join(", ", teamGroup.Select(c => c.Name + " " + c.Surname))
                                : "Brak trenera"
                        };

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(t => t.Team.TeamName.Contains(searchQuery));
            }

            Teams = await query.ToListAsync();
            return Page();

        }


        public async Task<IActionResult> OnPostAsync(string teamId)
        {
            return RedirectToPage("/ManageTeam", new {teamId});
        }

        }
}
