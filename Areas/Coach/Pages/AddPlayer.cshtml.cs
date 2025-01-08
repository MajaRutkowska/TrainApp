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
    public class AddPlayerModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddPlayerModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Team Team { get; set; }

        [BindProperty]
        public string TeamId { get; set; }
        public IList<ApplicationUser> Players { get; set; } = new List<ApplicationUser>();


        public async Task<IActionResult> OnGetAsync(string teamId)
        {

            Team = await _context.Team.FirstOrDefaultAsync(t => t.TeamId == teamId);


            Players = await (from user in _context.Users
                             join userRole in _context.UserRoles on user.Id equals userRole.UserId
                             join role in _context.Roles on userRole.RoleId equals role.Id
                             join teamUser in _context.TeamUsers on user.Id equals teamUser.UserId into userTeams
                             from teamUser in userTeams.DefaultIfEmpty()
                             where role.Name == "Player" && teamUser == null
                             select user).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string teamId, string playerId)
        {
            var team = await _context.Team.FirstOrDefaultAsync(t => t.TeamId == teamId);

            var player = await _context.Users.FirstOrDefaultAsync(u => u.Id == playerId);

            var exists = await _context.TeamUsers
                .AnyAsync(tu => tu.TeamId == teamId && tu.UserId == playerId);

            if (exists)
            {
                TempData["ErrorMessage"] = $"Zawodnik {player.Name} {player.Surname} jest już przypisany do drużyny {team.TeamName}.";
                return RedirectToPage("/AddPlayer", new { teamId });
            }

            var teamPlayer = new TeamUser
            {
                TeamId = team.TeamId,
                UserId = player.Id
            };
            _context.TeamUsers.Add(teamPlayer);
            await _context.SaveChangesAsync();

            return RedirectToPage("/AddPlayer", new {teamId});
        }
    }

}