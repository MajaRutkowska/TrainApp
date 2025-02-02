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

        public async Task OnGetAsync(string searchQuery, string selectedRole)
        {

            var usersQuery = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                usersQuery = usersQuery.Where(u => u.Name.Contains(searchQuery) || u.Surname.Contains(searchQuery));
            }

            Users = await usersQuery.ToListAsync();

            Roles = await _roleManager.Roles.ToListAsync();

     
            UserRoles = new Dictionary<string, List<string>>();

            foreach (var user in Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                UserRoles.Add(user.Id, roles.ToList());
            }

            if (!string.IsNullOrEmpty(selectedRole))
            {
                var filteredUserIds = UserRoles
                    .Where(ur => ur.Value.Contains(selectedRole))
                    .Select(ur => ur.Key)
                    .ToList();

                Users = Users.Where(u => filteredUserIds.Contains(u.Id)).ToList();
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
                
                var currentRoles = await _userManager.GetRolesAsync(user);

                
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

                if (removeResult.Succeeded)
                {
                    
                    var addResult = await _userManager.AddToRoleAsync(user, selectedRole);
                    if (addResult.Succeeded)
                    {
                        return RedirectToPage();
                    }
                    else
                    {
                        
                        foreach (var error in addResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    
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