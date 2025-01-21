using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TrainApp.Data;
using TrainApp.Data.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace TrainApp.Areas.Coach.Pages
{
    [Authorize(Roles = "Coach")]
    public class AddTrainingModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AddTrainingModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]

        public string Title { get; set; }

        [BindProperty]

        public DateTime Date { get; set; }

        [BindProperty]
        public string Color { get; set; }

        [BindProperty]

        public string TeamId { get; set; }

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

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var training = new Training
            {
                Title = Title,
                Date = Date,
                TeamId = TeamId,
                Color = Color
            };

            _context.Training.Add(training);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }

    }

}