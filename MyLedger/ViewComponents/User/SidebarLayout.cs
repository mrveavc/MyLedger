using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace MyLedger.ViewComponents.User
{
	public class SidebarLayout :ViewComponent
	{
		Context c = new Context();
		public IViewComponentResult Invoke() {

			var username = User.Identity.Name;
			var fullname=c.Users.Where(x=>x.UserName==username).Select(x=>x.FullName).FirstOrDefault();
			var image=c.Users.Where(x=>x.UserName == username).Select(x=>x.ImageUrl).FirstOrDefault();
			ViewBag.FullName = fullname;
			ViewBag.Image = image;
			return View(); 
		
		}
	}
}
