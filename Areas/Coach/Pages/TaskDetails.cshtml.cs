
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
using Microsoft.AspNetCore.Http;

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
        public float CompletionPercentage { get; set; }
        public List<Material> Materials { get; set; }
        public string FileName { get; set; }
        public List<ApplicationUser>Players { get; set; }
        [BindProperty]
        public List<IFormFile> UploadedFiles { get; set; }
        

        public async Task<IActionResult> OnGetAsync(string exerciseId)
        {
            Exercise = await _context.Exercise
            .Include(e => e.Team)
            .FirstOrDefaultAsync(e => e.ExerciseId == exerciseId);

            Materials = await _context.Material
                .Where(m => m.ExerciseId == exerciseId) .ToListAsync();

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


                CompletionPercentage = teamMembers.Count() > 0
                    ? (completedCount * 100) / teamMembers.Count : 0;

                CompletionStatus = teamMembers.Count > 0
                    ? $"Wykonanie: {(completedCount * 100) / teamMembers.Count}%"
                    : "Brak zawodników w drużynie.";

                Players = await (from userExercise in _context.UserExercise
                                 join user in _context.Users on userExercise.UserId equals user.Id
                                 where userExercise.ExerciseId == exerciseId && !userExercise.IsCompleted
                                 select user).ToListAsync();
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

        public async Task<IActionResult> OnPostAsync(string exerciseId)
        {
            if (UploadedFiles != null && UploadedFiles.Any())
            {
                foreach (var file in UploadedFiles)
                {
                    using var memoryStream = new MemoryStream();
                    await file.CopyToAsync(memoryStream);

                    var material = new Material
                    {
                        ExerciseId = exerciseId,
                        PdfFile = memoryStream.ToArray(),
                        FileName = file.FileName
                    };

                    _context.Material.Add(material);
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { exerciseId });
        }

        public async Task<IActionResult> OnGetDownloadMaterialAsync(string materialId)
        {
            var material = await _context.Material.FindAsync(materialId);

            if (material == null)
            {
                return NotFound();
            }

            return File(material.PdfFile, "application/pdf", material.FileName);
        }

    }
}
