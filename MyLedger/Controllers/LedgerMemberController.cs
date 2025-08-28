using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyLedger.Controllers
{
    //[Authorize(Roles = "Accountant,Admin")]

    public class LedgerMemberController : Controller
    {
        Context c = new Context();
        LedgerMemberManager mm= new LedgerMemberManager(new EfLedgerMemberRepository());
        LedgerManager lm = new LedgerManager(new EfLedgerRepository());
        UserManager um= new UserManager(new EfUserRepository());
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public LedgerMemberController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userName = User.Identity.Name;
            var userId = c.Users.Where(x => x.UserName == userName).Select(y => y.Id).FirstOrDefault();

            var values=mm.GetLedgerMemberListWithLedgerUser(userId);
            return View(values);
        }

        [HttpGet]
        public IActionResult AddMember()
        {
            var userName = User.Identity.Name;
            var userId = c.Users.Where(x => x.UserName == userName).Select(y => y.Id).FirstOrDefault();
            var values = lm.GetListWithLedgerName(userId);

            var userValues=c.Users
                .Where(x => x.Id != userId).ToList();

            List<SelectListItem> ledgervalues = values.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            })
                .ToList();
            List<SelectListItem> uservalues = userValues.Select(x => new SelectListItem
            {
                Text = x.FullName,
                Value = x.Id.ToString()
            })
               .ToList();


            ViewBag.lv = ledgervalues;
            ViewBag.uv=uservalues;
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> AddMember(LedgerMember p)
        {
            mm.TAdd(p);

            // Kullanıcıyı bul
            var user = await _userManager.FindByIdAsync(p.UserId.ToString());

            if (user != null)
            {
                // Eğer "Member" rolü yoksa önce oluştur
                if (!await _roleManager.RoleExistsAsync("Member"))
                {
                    var role = new AppRole
                    {
                        Name = "Member",
                        NormalizedName = "MEMBER"
                    };
                    await _roleManager.CreateAsync(role);
                }

                // Kullanıcının mevcut rollerini al
                var currentRoles = await _userManager.GetRolesAsync(user);

                if (currentRoles.Any())
                {
                    // Tüm rollerden çıkar
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                }

                // Yeni rol ata → Member
                await _userManager.AddToRoleAsync(user, "Member");
            }
            return RedirectToAction("Index", "LedgerMember");

        }
        public async Task<IActionResult> DeleteMember(int id,int UserId)
        {
      
            var user = await _userManager.FindByIdAsync(UserId.ToString());

            var values = mm.TGetById(id);
            mm.TDelete(values);

            // Kullanıcı hala başka LedgerMember kaydına sahip mi?
            bool hasOtherLedgerMemberships = c.LedgerMembers.Any(x => x.UserId == UserId);

            if (!hasOtherLedgerMemberships) // başka deftere bağlı değilse
            {
                // Eğer "Accountant" rolü yoksa önce oluştur
                if (!await _roleManager.RoleExistsAsync("Accountant"))
                {
                    var role = new AppRole
                    {
                        Name = "Accountant",
                        NormalizedName = "ACCOUNTANT"
                    };
                    await _roleManager.CreateAsync(role);
                }

                // Kullanıcının mevcut rollerini al
                var currentRoles = await _userManager.GetRolesAsync(user);

                if (currentRoles.Any())
                {
                    // Tüm rollerden çıkar
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                }

                // Yeni rol ata → Accountant
                await _userManager.AddToRoleAsync(user, "Accountant");
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult EditMember(int id)
        {
            var membervalues=mm.TGetById(id);
            var userName = User.Identity.Name;
            var userId = c.Users.Where(x => x.UserName == userName).Select(y => y.Id).FirstOrDefault();
            var values = lm.GetListWithLedgerName(userId);

            var userValues = c.Users
                .Where(x => x.Id != userId).ToList();
            List<SelectListItem> ledgervalues = values.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            })
              .ToList();
            List<SelectListItem> uservalues = userValues.Select(x => new SelectListItem
            {
                Text = x.FullName,
                Value = x.Id.ToString()
            })
               .ToList();


            ViewBag.lv = ledgervalues;
            ViewBag.uv = uservalues;
            return View(membervalues); 
        }
        [HttpPost]
        public IActionResult EditMember(LedgerMember p)
        {
            var membervalues = mm.TGetById(p.Id);
            membervalues.Id= p.Id;
            membervalues.LedgerId = p.LedgerId;
            membervalues.UserId = p.UserId;
            membervalues.Role = p.Role;
            membervalues.Permissions = p.Permissions;
            mm.TUpdate(p);

            return RedirectToAction("Index");
        }
    }
}
