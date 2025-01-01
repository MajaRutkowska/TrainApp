using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TrainApp.Data;
using TrainApp.Data.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;


namespace TrainApp.Areas.Admin.Pages
{
    public class UserRoleManageModel : PageModel
    {
        public readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRoleManageModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IList<ApplicationUser> Users { get; set; }
        public IList<IdentityRole> Roles { get; set; }

        public Dictionary<string, List<string>> UserRoles { get; set; }

        public async Task OnGetAsync()
        {
            // Pobieranie u¿ytkowników
            Users = await _userManager.Users.ToListAsync();

            // Pobieranie ról
            Roles = await _roleManager.Roles.ToListAsync();

            UserRoles = new Dictionary<string, List<string>>();

            foreach (var user in Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                UserRoles.Add(user.Id, roles.ToList());
            }
        }

        public async Task<IActionResult> OnPostAsync(string userId, string selectedRole)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(selectedRole))
            {
                return RedirectToPage();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // Pobieramy aktualne role u¿ytkownika
                var currentRoles = await _userManager.GetRolesAsync(user);

                // Usuwamy wszystkie przypisane role
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

                if (removeResult.Succeeded)
                {
                    // Dodajemy now¹ rolê
                    var addResult = await _userManager.AddToRoleAsync(user, selectedRole);
                    if (addResult.Succeeded)
                    {
                        return RedirectToPage();
                    }
                    else
                    {
                        // Obs³uguje b³êdy dodawania roli
                        foreach (var error in addResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    // Obs³uguje b³êdy usuwania ról
                    foreach (var error in removeResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return RedirectToPage();
        }
    }
}