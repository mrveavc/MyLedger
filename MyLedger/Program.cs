using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<Context>();
builder.Services.AddIdentity<AppUser, AppRole>(x =>
{
    x.Password.RequireUppercase = false;
    x.Password.RequireNonAlphanumeric = false;

}).AddEntityFrameworkStores<Context>().AddDefaultTokenProviders().AddRoles<AppRole>();
builder.Services.AddControllersWithViews();
builder.Services.AddMvc(config =>
{
    var policy = new AuthorizationPolicyBuilder()  // Projenin tümüne yetkilendirme verdiði için hiçbir sayfa açýlmaz.
                                                   // [AllowAnonymous] ile kurallardan muaf edebiliriz (LoginController)
                                                   // Oturum açýlmayýnca tüm urllerde logine yönlendirme yapýlýyor. 
               .RequireAuthenticatedUser()
               .Build();
    config.Filters.Add(new AuthorizeFilter(policy));

});

builder.Services.AddMvc();
builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme
    ).AddCookie(x =>
    {
        x.LoginPath = "/Login/Index";
    });
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(100); // 100 dk oturum açýk kalma süresi
    options.AccessDeniedPath = new PathString("/Login/AccessDenied");
    options.LoginPath = "/Login/Index";
    options.SlidingExpiration = true;
});
		
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/ErrorPage/Error1", "?code={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
		name: "areas",
		pattern: "{area:exists}/{controller=AdminDashboard}/{action=Index}/{id?}");

	endpoints.MapControllerRoute(
		name: "default",
		pattern: "{controller=Login}/{action=Index}/{id?}");
});
//app.MapControllerRoute(
//	name: "areas",
//	pattern: "{area:exists}/{controller=AdminDashboard}/{action=Index}/{id?}"
//	);
//app.MapControllerRoute(
//	name: "default",
//	pattern: "{controller=Login}/{action=Index}/{id?}"
//	);



app.Run();
