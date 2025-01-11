
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
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TrainApp.Areas.Player.Pages
{
    [Authorize(Roles ="Player")]
    public class TaskDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public TaskDetailsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        public Exercise Exercise { get; set; }
        public string CreatedBy { get; set; }
        public bool IsCompleted { get; set; }
        public string PlayerId { get; set; }
        public List<Material> Materials { get; set; }

        public async Task<IActionResult> OnGetAsync(string exerciseId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            Exercise = await _context.Exercise.FirstOrDefaultAsync(e => e.ExerciseId == exerciseId);

            var createdByUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == Exercise.CreatedBy);

            if (createdByUser != null)
            {
                CreatedBy = $"{createdByUser.Name} {createdByUser.Surname}";
            }
            else
            {
                CreatedBy = "Nieznany użytkownik";
            }

            var userExercise = await _context.UserExercise
               .FirstOrDefaultAsync(ue => ue.ExerciseId == exerciseId && ue.UserId == currentUser.Id);

            IsCompleted = userExercise.IsCompleted;
            PlayerId = currentUser.Id;
            Materials = await _context.Material
                .Where(m => m.ExerciseId == exerciseId).ToListAsync();



            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string exerciseId, bool isCompleted)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var userExercise = await _context.UserExercise
                .FirstOrDefaultAsync(ue => ue.ExerciseId == exerciseId && ue.UserId == currentUser.Id);

            userExercise.IsCompleted = isCompleted;
            await _context.SaveChangesAsync();

            return RedirectToPage(new { exerciseId });
        }

        public async Task<IActionResult> OnGetDownloadAsync(string materialId)
        {
            var material = await _context.Material
                .FirstOrDefaultAsync(m => m.MaterialId == materialId);

            if (material == null)
            {
                return NotFound(); // Jeśli materiał nie istnieje, zwróć 404
            }

            return File(material.PdfFile, "application/pdf", material.FileName);
        }

    }
}
