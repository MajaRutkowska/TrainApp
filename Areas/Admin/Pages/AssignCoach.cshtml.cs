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


        public async Task <IActionResult> OnGetAsync( string teamId, string searchQuery)
        {

            Team = await _context.Team.FirstOrDefaultAsync(t => t.TeamId == teamId);


            var query = _context.Users
        .Join(_context.UserRoles, user => user.Id, userRole => userRole.UserId, (user, userRole) => new { user, userRole })
        .Join(_context.Roles, ur => ur.userRole.RoleId, role => role.Id, (ur, role) => new { ur.user, role })
        .Where(x => x.role.Name == "Coach");

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(x => x.user.Name.Contains(searchQuery) || x.user.Surname.Contains(searchQuery));
            }

            Coaches = await query.Select(x => x.user).ToListAsync();

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