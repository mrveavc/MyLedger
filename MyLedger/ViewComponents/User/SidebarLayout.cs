using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyLedger.ViewComponents.User
{
	public class SidebarLayout : ViewComponent
	{
		Context c = new Context();
        private readonly UserManager<AppUser> _userManager;
        public SidebarLayout(Context context, UserManager<AppUser> userManager)
        {
            c = context;
            _userManager = userManager;
        }
        public IViewComponentResult Invoke() {

            var username = User.Identity.Name;
            var userId = c.Users.Where(x => x.UserName == username).Select(x => x.Id).FirstOrDefault();

            var fullname = c.Users.Where(x => x.UserName == username).Select(x => x.FullName).FirstOrDefault();
            var image = c.Users.Where(x => x.UserName == username).Select(x => x.ImageUrl).FirstOrDefault();
			ViewBag.FullName = fullname;
			ViewBag.Image = image;
			//if (userId != null)
			//{
			//    ViewBag.FullName = fullname;
			//    ViewBag.Image = image;

			//    // Kullanıcının Member rolünde Ledger üyesi olup olmadığını kontrol et
			//    bool isMember = c.LedgerMembers.Any(x => x.UserId == userId && x.Role == "Member");
			//    ViewBag.IsMember = isMember; // true = üye, false = üye değil
			//}

			return View(); 
		
		}
	}
}
