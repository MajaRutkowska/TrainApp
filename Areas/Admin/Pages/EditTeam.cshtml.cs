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

namespace TrainApp.Areas.Admin.Pages
{
    [Authorize(Roles ="Admin")]
    public class EditTeamModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AddTeamModel> _logger;
        public EditTeamModel(ApplicationDbContext context, ILogger<AddTeamModel> logger )
        {
            _context = context;
            _logger = logger;
        }

        public Team Team { get; set; }

        [BindProperty]
        public string Street { get; set; }

        [BindProperty]
        public string City { get; set; }

        [BindProperty]
        public string PostalCode { get; set; }

        [BindProperty]
        public Team TeamToEdit { get; set; } = new Team();
        public IActionResult OnGet(string teamId)
        {
            Team = _context.Team.FirstOrDefault(t => t.TeamId == teamId);
            var addressParts = Team.Address?.Split(',') ?? new string[3];
            Street = addressParts.ElementAtOrDefault(0)?.Trim();
            City = addressParts.ElementAtOrDefault(1)?.Trim();
            PostalCode = addressParts.ElementAtOrDefault<string>(2)?.Trim();
            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string teamId)
        {
           
            if (ModelState.IsValid)
            {
                return Page();
            }
            Team =_context.Team.FirstOrDefault(t => t.TeamId==teamId);

            Team.TeamName = TeamToEdit.TeamName;
            Team.Address = $"{Street}, {City}, {PostalCode}";
            Team.CreationYear = TeamToEdit.CreationYear;

            _context.Attach(Team).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToPage("/ManageTeam", new {teamId});
        }

        }
}
