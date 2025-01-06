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


namespace TrainApp.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class AssignChoachModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AssignChoachModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Team Team { get; set; }

        [BindProperty]
        public string TeamId { get; set; }
        public IList<ApplicationUser> Coaches { get; set; } = new List<ApplicationUser>();


        public async Task <IActionResult> OnGetAsync( string teamId)
        {

            Team = await _context.Team.FirstOrDefaultAsync(t => t.TeamId == teamId);

            
            Coaches = await (from user in _context.Users
                             join userRole in _context.UserRoles on user.Id equals userRole.UserId
                             join role in _context.Roles on userRole.RoleId equals role.Id
                             where role.Name == "Coach"
                             select user).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string teamId, string coachId)
        {
            var team = await _context.Team.FirstOrDefaultAsync(t => t.TeamId == teamId);

            var coach = await _context.Users.FirstOrDefaultAsync(u => u.Id == coachId);

            var exists = await _context.TeamUsers
                .AnyAsync(tu => tu.TeamId == teamId && tu.UserId == coachId);

            if (exists)
            {
                TempData["ErrorMessage"] = $"Trener {coach.Name} {coach.Surname} jest ju¿ przypisany do dru¿yny {team.TeamName}.";
                return RedirectToPage("/AssignCoach", new {teamId});
            }

            var teamCoach = new TeamUser
            {
                TeamId = team.TeamId,
                UserId = coach.Id
            };
            _context.TeamUsers.Add(teamCoach);
            await _context.SaveChangesAsync();

            return RedirectToPage("/ManageTeam", new {teamId});
        }
    }
    
}