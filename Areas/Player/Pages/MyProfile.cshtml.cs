
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

namespace TrainApp.Areas.Player.Pages
{
    [Authorize(Roles ="Player")]
    public class MyProfileModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MyProfileModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public ApplicationUser Player { get; set; }
        public Parameters PlayerParameters { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            Player = await _context.Users.FirstOrDefaultAsync(t => t.Id == currentUser.Id);
            PlayerParameters = await _context.Parameters.FirstOrDefaultAsync(p => p.UserId == currentUser.Id);


            return Page();
        }

    }
}
