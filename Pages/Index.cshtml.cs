using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainApp.Data.Models;
using TrainApp.Data;
using Microsoft.AspNetCore.Identity;
using System.Drawing;

namespace TrainApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager; 

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public List<Training> Trainings { get; set; }
        public List<Team> Teams { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);


            if (currentUser == null || (!User.IsInRole("Coach") && !User.IsInRole("Player")))
            {

                return Page();
            }

            Teams = await _context.TeamUsers
                .Where(tu => tu.UserId == currentUser.Id)
                .Select(tu => tu.Team)
                .ToListAsync();

            var trainings = await (from training in _context.Training
                                   join teamUser in _context.TeamUsers on training.TeamId equals teamUser.TeamId
                                   where teamUser.UserId == currentUser.Id
                                   select new
                                   {
                                       id = training.TrainingId,
                                       title = training.Title,
                                       start = training.Date.ToString("yyyy-MM-ddTHH:mm:ss"),  
                                       end = training.Date.AddHours(1).ToString("yyyy-MM-ddTHH:mm:ss"),
                                       description = training.Title,
                                       color = training.Color
                                   }).ToListAsync();

            ViewData["Trainings"] = trainings;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string title, DateTime date, string teamId, string color)
        {
            var training = new Training
            {
                Title = title,
                Date = date,
                TeamId = teamId, 
                Color = color
            };

            _context.Training.Add(training);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }


        public async Task<IActionResult> OnPostDeleteTrainingAsync([FromBody] DeleteTrainingRequest request)
        {
            try
            {
                if (request == null || request.Id == String.Empty)
                {
                    return new JsonResult(new { success = false, message = "Nieprawid³owe ID treningu" });
                }

                var training = await _context.Training.FindAsync(request.Id);

                if (training == null)
                {
                    return new JsonResult(new { success = false, message = "Nie znaleziono treningu" });
                }


                _context.Training.Remove(training);
                await _context.SaveChangesAsync();

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine("B³¹d: " + ex.Message);  // Logowanie b³êdu
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        public class DeleteTrainingRequest
        {
            public string Id { get; set; }
        }
    }
}
