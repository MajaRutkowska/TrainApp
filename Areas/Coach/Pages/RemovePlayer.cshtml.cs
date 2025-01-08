// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

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
    public class RemovePlayerModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RemovePlayerModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public ApplicationUser Player { get; set; }
        public Team Team { get; set; }



        public async Task<IActionResult> OnGetAsync(string playerId, string teamId)
        {
            Player = await _context.Users.FirstOrDefaultAsync(u => u.Id == playerId);
            Team = await _context.Team.FirstOrDefaultAsync(t => t.TeamId == teamId);

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string playerId, string teamId)
        {
            // Pobieramy drużynę i zawodnika
            var teamUser = await _context.TeamUsers.FirstOrDefaultAsync(tu => tu.UserId == playerId && tu.TeamId == teamId);

            if (teamUser != null)
            {
                // Usuwamy zawodnika z drużyny
                _context.TeamUsers.Remove(teamUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/TeamDetails", new { teamid = teamId });
        }

    }
}
