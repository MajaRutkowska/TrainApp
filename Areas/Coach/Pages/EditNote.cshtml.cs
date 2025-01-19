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
    public class EditNoteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditNoteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Notes Note { get; set; }
        public string PlayerId { get; set; }

        public async Task<IActionResult> OnGetAsync(string noteId)
        {
            Note = await _context.Notes.FindAsync(noteId);

            if (Note == null)
            {
                return NotFound();
            }

            PlayerId = Note.PlayerId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string noteId, string noteText)
        {
            var note = await _context.Notes.FindAsync(noteId);

            if (note != null)
            {
                note.Text = noteText;
                _context.Notes.Update(note);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/PlayerDetails", new { playerId = note.PlayerId });
        }
    }

}