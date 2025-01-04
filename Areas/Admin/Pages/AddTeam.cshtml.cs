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
    public class AddTeamModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AddTeamModel> _logger;
        public AddTeamModel(ApplicationDbContext context, ILogger<AddTeamModel> logger )
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public string Street { get; set; }

        [BindProperty]
        public string City { get; set; }

        [BindProperty]
        public string PostalCode { get; set; }

        [BindProperty]
        public Team NewTeam { get; set; } = new Team();
        public IActionResult Create()
        {
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
           
            if (ModelState.IsValid || _context.Team == null || NewTeam == null)
            {
                return Page();
            }

            NewTeam.Address = $"{Street}, {City}, {PostalCode}";
            _context.Team.Add(NewTeam);
            await _context.SaveChangesAsync();
            return RedirectToPage("/Index");
        }

        }
}
