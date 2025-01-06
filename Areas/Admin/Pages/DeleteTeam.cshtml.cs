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
    public class DeleteTeamModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AddTeamModel> _logger;
        public DeleteTeamModel(ApplicationDbContext context, ILogger<AddTeamModel> logger )
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public Team Team { get; set; }

       
        public async Task <IActionResult> OnGetAsync(string teamId)
        {
          Team = _context.Team.FirstOrDefault(t => t.TeamId == teamId);
            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string teamId)
        {
           var teamToDelete = _context.Team.FirstOrDefault(t => t.TeamId == teamId);
           _context.Team.Remove(teamToDelete);
            await _context.SaveChangesAsync();
            
            return RedirectToPage("/Teams");
        }

        }
}
