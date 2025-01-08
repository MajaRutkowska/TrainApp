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
    public class TaskListModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public TaskListModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IEnumerable<dynamic> TeamsExercises { get; set; }
        public IEnumerable<dynamic> PlayersExercises { get; set; } 

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            TeamsExercises = await (from exercise in _context.Exercise
                                    where exercise.CreatedBy == currentUser.Id && exercise.TeamId != null
                                    join team in _context.Team on exercise.TeamId equals team.TeamId
                                    select new
                                    {
                                        ExerciseId = exercise.ExerciseId,
                                        Title = exercise.Title,
                                        Description = exercise.Description,
                                        TeamId = exercise.TeamId,
                                        TeamName = team.TeamName
                                    }).ToListAsync();



            PlayersExercises = await (from userExercise in _context.UserExercise
                                          join exercise in _context.Exercise on userExercise.ExerciseId equals exercise.ExerciseId
                                          join user in _context.Users on userExercise.UserId equals user.Id
                                          where exercise.CreatedBy == currentUser.Id && exercise.TeamId == null
                                      select new
                                          {
                                              ExerciseId = exercise.ExerciseId,
                                              Exercise = exercise,
                                              Player = user
                                          }).ToListAsync();

            return Page();

        }


        public async Task<IActionResult> OnPostAsync(string teamId)
        {
            return RedirectToPage("/AddTask");
        }

    }
}
