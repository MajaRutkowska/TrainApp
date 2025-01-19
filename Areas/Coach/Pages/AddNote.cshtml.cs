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
    public class AddNoteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AddNoteModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Notes Note { get; set; }
        [BindProperty]
        public string NoteText { get; set; }

        [BindProperty]
        public string PlayerId { get; set; }

        public async Task<IActionResult> OnGetAsync(string playerId)
        {
            
            PlayerId = playerId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string playerId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            Note = new Notes
            {
                PlayerId = PlayerId,
                Text = NoteText,
                CoachId = currentUser.Id,
                CreationDate = DateTime.Now
            };
            _context.Notes.Add(Note);
            await _context.SaveChangesAsync();

            return RedirectToPage("PlayerDetails", new { playerId = PlayerId });
        }
    }

}