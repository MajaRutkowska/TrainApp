
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
    public class TaskDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;


        public TaskDetailsModel(ApplicationDbContext context)
        {
            _context = context;

        }

        public Exercise Exercise { get; set; }
        public string AssignedEntity { get; set; } 
        public string AssignedEntityName { get; set; } 
        public string CompletionStatus { get; set; }

        public async Task<IActionResult> OnGetAsync(string exerciseId)
        {
            Exercise = await _context.Exercise
            .Include(e => e.Team)
            .FirstOrDefaultAsync(e => e.ExerciseId == exerciseId);

            if (Exercise.TeamId != null)
            {
                AssignedEntity = "Team";
                AssignedEntityName = Exercise.Team.TeamName;

                var teamMembers = await _context.TeamUsers
                    .Where(tu => tu.TeamId == Exercise.TeamId)
                    .Select(tu => tu.UserId)
                    .ToListAsync();

                var completedCount = await _context.UserExercise
                    .Where(ue => ue.ExerciseId == exerciseId && ue.IsCompleted)
                    .CountAsync();

                CompletionStatus = teamMembers.Count > 0
                    ? $"Wykonanie: {(completedCount * 100) / teamMembers.Count}%"
                    : "Brak zawodników w drużynie.";
            }
            else
            {
                AssignedEntity = "Player";

                var userExercise = await _context.UserExercise
                    .Include(ue => ue.ApplicationUser)
                    .FirstOrDefaultAsync(ue => ue.ExerciseId == exerciseId);

                if (userExercise != null)
                {
                    AssignedEntityName = $"{userExercise.ApplicationUser.Name} {userExercise.ApplicationUser.Surname}";
                    CompletionStatus = userExercise.IsCompleted ? "Zadanie wykonane" : "Zadanie nie zostało wykonane";
                }
                else
                {
                    AssignedEntityName = "Nieznany zawodnik";
                    CompletionStatus = "Brak danych o wykonaniu.";
                }
            }

            return Page();
        }

    }
}
