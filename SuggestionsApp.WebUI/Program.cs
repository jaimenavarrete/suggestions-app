using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuggestionsApp.Models.Data.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
var configutation = builder.Configuration;

var connectionString = configutation.GetConnectionString("SuggestionsApp");

services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
services.AddDefaultIdentity<ApplicationUser>(options => 
    options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationContext>();

services.AddControllersWithViews().AddRazorRuntimeCompilation();
services.AddRazorPages();

// Configure the HTTP request pipeline.
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapRazorPages();
});

app.Run();
