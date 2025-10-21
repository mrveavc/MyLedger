using System.Data;
using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MyLedger.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        Context c=new Context();
        UserManager um = new UserManager(new EfUserRepository());
        private readonly UserManager<AppUser> _userManager;
        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {

            var values =um.GetList();
            var nonAdminUsers = new List<AppUser>();
            var userRoles = new Dictionary<int, string>(); 
            foreach (var user in values)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var roleName = roles.FirstOrDefault() ?? "Rol Yok";

                if (roleName != "Admin")
                {
                    nonAdminUsers.Add(user);
                    userRoles[user.Id] = roleName; 

                }
            }

            ViewBag.UserRoles = userRoles;
            return View(nonAdminUsers);
        }
        public IActionResult deleteUser(int id)
        {
            var valuıes=um.TGetById(id);
            um.TDelete(valuıes);
            return RedirectToAction("Index", "User", new { area = "Admin" });

        }
        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var user = um.TGetById(id);

            var roles = await _userManager.GetRolesAsync(user);
            var roleName = roles.FirstOrDefault() ?? "Rol Yok";

            // 3. Role bilgisini ViewBag veya model ile gönder
            ViewBag.RoleName = roleName;

            return View(user);
        }
        [HttpPost]
        public IActionResult EditUser(AppUser p)
        {
            var uservalue = um.TGetById(p.Id);
            if (uservalue != null)
            {
                uservalue.FullName = p.FullName;
                uservalue.Email = p.Email;

                um.TUpdate(uservalue);
            }


            return RedirectToAction("Index");
        }

    }
}
