
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
    public class PlayerDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PlayerDetailsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public ApplicationUser Player { get; set; }
        public Parameters PlayerParameters { get; set; }
        public Team Team { get; set; }
        public TeamUser teamUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string playerId)
        {
            Player = await _context.Users.FirstOrDefaultAsync(t => t.Id == playerId);
            PlayerParameters = await _context.Parameters.FirstOrDefaultAsync(p => p.UserId == playerId);
            teamUser = await _context.TeamUsers.FirstOrDefaultAsync(u => u.UserId ==  playerId);

            return Page();
        }

    }
}
