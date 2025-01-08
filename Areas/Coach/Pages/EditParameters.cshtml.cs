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
    public class EditParametersModel : PageModel
    {
        private readonly ApplicationDbContext _context;


        public EditParametersModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string PlayerId { get; set; }

        public ApplicationUser Player { get; set; }
        public Parameters Parameters { get; set; }
        public async Task<IActionResult> OnGetAsync(string playerId)
        {

            Player = await _context.Users.FirstOrDefaultAsync(t => t.Id == playerId);
            Parameters = await _context.Parameters.FirstOrDefaultAsync(p => p.UserId == playerId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string playerId, float height, float weight, float speed)
        {

            var parameters = await _context.Parameters.FirstOrDefaultAsync(p => p.UserId == playerId);

            if (parameters == null)
            {

                parameters = new Parameters
                {
                    UserId = playerId,
                    Height = height,
                    Weight = weight,
                    Speed = speed
                };
                _context.Parameters.Add(parameters);
            }

            else
            {
                parameters.Height = height;
                parameters.Weight = weight;
                parameters.Speed = speed;
                _context.Parameters.Update(parameters);
            }


    await _context.SaveChangesAsync();


            return RedirectToPage("/PlayerDetails", new { playerId = playerId});
        }
    }

}