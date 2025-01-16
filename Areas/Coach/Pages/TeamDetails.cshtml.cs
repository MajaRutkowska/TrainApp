
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
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout;
using iText.Kernel.Font;
using Org.BouncyCastle.Crypto;
using iText.Bouncycastleconnector;

namespace TrainApp.Areas.Coach.Pages
{
    [Authorize(Roles ="Coach")]
    public class TeamDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TeamDetailsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Team Team { get; set; }
        public List<ApplicationUser> Players { get; set; } = new List<ApplicationUser>();

        public async Task<IActionResult> OnGetAsync(string teamId, bool exportPdf = false)
        {
            if (exportPdf)
            {
                return await OnGetExportPdfAsync(teamId);
            }

            Team = _context.Team.FirstOrDefault(t => t.TeamId == teamId);

            Players = await (from user in _context.Users
                             join userRole in _context.UserRoles on user.Id equals userRole.UserId
                             join role in _context.Roles on userRole.RoleId equals role.Id
                             join teamUser in _context.TeamUsers on user.Id equals teamUser.UserId
                             where role.Name == "Player" && teamUser.TeamId == teamId
                             select user).ToListAsync();


            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string playerId, string teamId)
        {
            var teamUser = await _context.TeamUsers.FirstOrDefaultAsync(tu => tu.UserId == playerId && tu.TeamId == teamId);

            if (teamUser != null)
            {
                _context.TeamUsers.Remove(teamUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Coach/TeamDetails", new { teamid = teamId });
        }

        private async Task<IActionResult> OnGetExportPdfAsync(string teamId)
        {
            Team = _context.Team.FirstOrDefault(t => t.TeamId == teamId);

            Players = await (from user in _context.Users
                             join userRole in _context.UserRoles on user.Id equals userRole.UserId
                             join role in _context.Roles on userRole.RoleId equals role.Id
                             join teamUser in _context.TeamUsers on user.Id equals teamUser.UserId
                             where role.Name == "Player" && teamUser.TeamId == teamId
                             select user).ToListAsync();

            using var memoryStream = new MemoryStream();

            var writer = new PdfWriter(memoryStream);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            var boldFont = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
            document.Add(new Paragraph("Lista zawodników").SetFont(boldFont));
            document.Add(new Paragraph(" ")); 

            foreach (var player in Players)
            {
                document.Add(new Paragraph($"{player.Name} {player.Surname}, Data urodzenia: {player.BirthDate:yyyy-MM-dd}"));
            }

            document.Close();

            return File(memoryStream.ToArray(), "application/pdf", $"ListaZawodnikow_{Team.TeamName}.pdf");
        }

    }
}
