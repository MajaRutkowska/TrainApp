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
    [Authorize(Roles = "Player")]
    public class MyTasksModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public MyTasksModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IEnumerable<dynamic> TeamExercises { get; set; }
        public List<Exercise> PlayerExercises { get; set; } = new List<Exercise>();


        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);


            TeamExercises = await (from userExercise in _context.UserExercise
                                   join exercise in _context.Exercise on userExercise.ExerciseId equals exercise.ExerciseId
                                   join team in _context.Team on exercise.TeamId equals team.TeamId
                                   where userExercise.UserId == currentUser.Id && exercise.TeamId != null
                                   select new
                                   {
                                       ExerciseId = exercise.ExerciseId,
                                       Title = exercise.Title,
                                       Description = exercise.Description,
                                       TeamName = team.TeamName 
                                   }).ToListAsync();


            PlayerExercises = await (from userExercise in _context.UserExercise
                                     join exercise in _context.Exercise on userExercise.ExerciseId equals exercise.ExerciseId
                                     where userExercise.UserId == currentUser.Id && exercise.TeamId == null
                                     select exercise).ToListAsync();

            return Page();

        }

    }
}
