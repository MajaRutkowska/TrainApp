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
    [Authorize(Roles = "Coach")]
    public class UndonePlayersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public UndonePlayersModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public Exercise Exercise { get; set; }
        public List<ApplicationUser> Players { get; set; }

        public async Task<IActionResult> OnGetAsync(string exerciseId)
        {

            Exercise = await _context.Exercise
                .Include(e => e.Team)
                .FirstOrDefaultAsync(e => e.ExerciseId == exerciseId);

            if (Exercise == null || Exercise.TeamId == null)
            {
                return NotFound();
            }

            Players = await (from userExercise in _context.UserExercise
                                    join user in _context.Users on userExercise.UserId equals user.Id
                                    where userExercise.ExerciseId == exerciseId && !userExercise.IsCompleted
                                    select user)
                                    .ToListAsync();

            return Page();
        }
    }
}

