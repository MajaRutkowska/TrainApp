using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

namespace TrainApp.Areas.Admin.Pages
{
    public class SendEmailModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public SendEmailModel(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }


        public IEnumerable<dynamic> Trainers { get; set; }

        // Dane formularza
        [BindProperty]
        public List<string> SelectedTrainers { get; set; }
        [BindProperty]
        public string Subject { get; set; }
        [BindProperty]
        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Pobierz trenerów z bazy danych
            var trainers = await (from user in _context.Users
                                  join userRole in _context.UserRoles on user.Id equals userRole.UserId
                                  join role in _context.Roles on userRole.RoleId equals role.Id
                                  where role.Name == "Coach"
                                  select new
                                  {
                                      Name = $"{user.Name} {user.Surname}",
                                      Email = user.Email
                                  }).ToListAsync();
            Trainers = trainers;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {

                var trainers = await (from user in _context.Users
                                  join userRole in _context.UserRoles on user.Id equals userRole.UserId
                                  join role in _context.Roles on userRole.RoleId equals role.Id
                                  where role.Name == "Coach"
                                  select new 
                                  {
                                      Name = $"{user.Name} {user.Surname}",
                                      Email = user.Email
                                  }).ToListAsync();
                Trainers = trainers;

                return Page();
            }

            // Wysyłanie e-maili do wybranych trenerów
            foreach (var email in SelectedTrainers)
            {
                await _emailSender.SendEmailAsync(email, Subject, Message);
            }

            // Zwrócenie komunikatu po pomyślnym wysłaniu
            TempData["SuccessMessage"] = "E-mail został pomyślnie wysłany do wybranych trenerów.";
            return RedirectToPage();
        }
    }

    // Model widoku trenera
    public class TrainerViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}