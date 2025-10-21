using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyLedger.Areas.Admin.Models;

namespace MyLedger.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public RoleController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var userRoleList = new List<(AppUser user, string roleName)>();

            foreach (var user in users)
            {
                // Kullanıcının rollerini al
                var roles = await _userManager.GetRolesAsync(user);
                var roleName = roles.FirstOrDefault() ?? "Rol Yok";

                userRoleList.Add((user, roleName));
            }

            return View(userRoleList); // Razor tarafında tuple listesi gönderiyoruz
        }
        public async Task<IActionResult> DeleteRole(string id)
        {
			var user = await _userManager.FindByIdAsync(id);
			if (user != null)
			{
				await _userManager.DeleteAsync(user);
			}
			return RedirectToAction("Index");
			return View();
        }
        [HttpGet]
		public async Task<IActionResult> EditRole(string id)
		{
            var user = _userManager.FindByIdAsync(id).Result; // AppUser
            var roles = _roleManager.Roles.ToList(); // Roller

            var model = new EditUserViewModel
            {
                Id = user.Id.ToString(),
                FullName = user.FullName,
                Email = user.Email,
                //ImageUrl = user.ImageUrl,
                SelectedRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault()
            };

            ViewBag.Roles = roles; // Razor sayfasında select list için

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                user.FullName = model.FullName;
                user.Email = model.Email;
                user.UserName = model.Email; // gerekirse
                await _userManager.UpdateAsync(user);

                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!string.IsNullOrEmpty(model.SelectedRole))
                {
                    await _userManager.AddToRoleAsync(user, model.SelectedRole);
                }
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
		public async Task<IActionResult> AddRole(AppUser user, string selectedRole)
		{
			var result = await _userManager.CreateAsync(user, "DefaultPassword123!"); // şifre örnek
			if (result.Succeeded)
			{
				if (!string.IsNullOrEmpty(selectedRole))
					await _userManager.AddToRoleAsync(user, selectedRole);

				return RedirectToAction("Index");
			}

			ViewBag.Roles = _roleManager.Roles.ToList();
			return View(user);
		}


	}
}
