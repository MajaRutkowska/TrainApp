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


namespace TrainApp.Areas.Coach.Pages
{
    [Authorize(Roles = "Coach")]
    public class AddTaskModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AddTaskModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Team>Teams { get; set; } = new List<Team>();
       public List<ApplicationUser> Players { get; set; } = new List<ApplicationUser>();

        [BindProperty]
        public string TaskTitle { get; set; }

        [BindProperty]
        public string TaskDescription { get; set; }
        [BindProperty]
        public DateTime TaskEndDate{ get; set; }

        [BindProperty]
        public string SelectedTeamId { get; set; }

        [BindProperty]
        public string SelectedPlayerId { get; set; }

        [BindProperty]
        public string TaskType { get; set; }


        public async Task<IActionResult> OnGetAsync(string teamId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            Teams = await (from team in _context.Team
                           join teamUser in _context.TeamUsers on team.TeamId equals teamUser.TeamId
                           where teamUser.UserId == currentUser.Id 
                           select team).ToListAsync();


            Players = await (from user in _context.Users
                             join userRole in _context.UserRoles on user.Id equals userRole.UserId
                             join role in _context.Roles on userRole.RoleId equals role.Id
                             join teamUser in _context.TeamUsers on user.Id equals teamUser.UserId 
                             where role.Name == "Player" 
                             select user).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(TaskTitle) || string.IsNullOrEmpty(TaskDescription))
            {
                TempData["ErrorMessage"] = "Tytuł i opis zadania są wymagane!";
                return RedirectToPage(); 
            }
            var currentUser = await _userManager.GetUserAsync (User);

            var exercise = new Exercise
            {
                Title = TaskTitle,
                Description = TaskDescription,
                EndDate = TaskEndDate,
                TeamId = TaskType == "Team" ? SelectedTeamId : null, 
                CreatedBy = currentUser.Id
            };


            _context.Exercise.Add(exercise);
            await _context.SaveChangesAsync();


            if (TaskType == "Team" && !string.IsNullOrEmpty(SelectedTeamId))
            {

                var teamPlayers = await (from teamUser in _context.TeamUsers
                                         join userRole in _context.UserRoles on teamUser.UserId equals userRole.UserId
                                         join role in _context.Roles on userRole.RoleId equals role.Id
                                         where teamUser.TeamId == SelectedTeamId && role.Name == "Player"
                                         select teamUser.ApplicationUser).ToListAsync();


                foreach (var player in teamPlayers)
                {
                    var userExercise = new UserExercise
                    {
                        UserId = player.Id,
                        ExerciseId = exercise.ExerciseId
                    };

                    _context.UserExercise.Add(userExercise);
                }

                await _context.SaveChangesAsync();
            }
            else if (TaskType == "Player" && !string.IsNullOrEmpty(SelectedPlayerId))
            {
                
                var player = await _context.Users.FindAsync(SelectedPlayerId);
                if (player != null)
                {
                    var userExercise = new UserExercise
                    {
                        UserId = player.Id,
                        ExerciseId = exercise.ExerciseId
                    };

                    _context.UserExercise.Add(userExercise);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("/TaskList"); 
        }
    }

}