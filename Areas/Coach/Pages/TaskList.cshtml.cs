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

        public async Task<IActionResult> OnGetAsync(string searchQuery, string statusFilter)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var teamsExercisesQuery = from exercise in _context.Exercise
                                      where exercise.CreatedBy == currentUser.Id && exercise.TeamId != null
                                      join team in _context.Team on exercise.TeamId equals team.TeamId
                                      let completedPercentage = _context.UserExercise
                                          .Where(ue => ue.ExerciseId == exercise.ExerciseId && ue.IsCompleted)
                                          .Count() * 100 /
                                          (_context.TeamUsers
                                              .Where(tu => tu.TeamId == exercise.TeamId)
                                              .Count() > 0
                                              ? _context.TeamUsers.Where(tu => tu.TeamId == exercise.TeamId).Count()
                                              : 1)
                                      select new
                                      {
                                          ExerciseId = exercise.ExerciseId,
                                          Title = exercise.Title,
                                          TeamName = team.TeamName,
                                          CompletedPercentage = completedPercentage,
                                          Status = completedPercentage == 100 ? "Wykonane" : "Niewykonane"
                                      };


            var playersExercisesQuery = from userExercise in _context.UserExercise
                                        join exercise in _context.Exercise on userExercise.ExerciseId equals exercise.ExerciseId
                                        join user in _context.Users on userExercise.UserId equals user.Id
                                        where exercise.CreatedBy == currentUser.Id && exercise.TeamId == null
                                        select new
                                        {
                                            ExerciseId = exercise.ExerciseId,
                                            Exercise = exercise,
                                            Player = user,
                                            IsCompleted = userExercise.IsCompleted,
                                            Status = userExercise.IsCompleted ? "Wykonane" : "Niewykonane"
                                        };

            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                teamsExercisesQuery = teamsExercisesQuery.Where(e => e.Title.ToLower().Contains(searchQuery));
                playersExercisesQuery = playersExercisesQuery.Where(e => e.Exercise.Title.ToLower().Contains(searchQuery));
            }

            if (!string.IsNullOrEmpty(statusFilter))
            {
                if (statusFilter == "Wykonane")
                {
                    teamsExercisesQuery = teamsExercisesQuery.Where(e => e.Status == "Wykonane");
                    playersExercisesQuery = playersExercisesQuery.Where(e => e.Status == "Wykonane");
                }
                else if (statusFilter == "Niewykonane")
                {
                    teamsExercisesQuery = teamsExercisesQuery.Where(e => e.Status == "Niewykonane");
                    playersExercisesQuery = playersExercisesQuery.Where(e => e.Status == "Niewykonane");
                }
            }

            TeamsExercises = await teamsExercisesQuery.ToListAsync();
            PlayersExercises = await playersExercisesQuery.ToListAsync();

            return Page();

        }


        public async Task<IActionResult> OnPostAsync(string teamId)
        {
            return RedirectToPage("/AddTask");
        }

    }
}
