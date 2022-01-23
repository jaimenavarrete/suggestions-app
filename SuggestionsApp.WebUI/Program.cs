using Microsoft.EntityFrameworkCore;
using SuggestionsApp.Models.Data.Database;
using SuggestionsApp.Models.Data.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
var configutation = builder.Configuration;

var connectionString = configutation.GetConnectionString("SuggestionsApp");

// Identity DB Context
services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
// SuggestionsApp DB Context
services.AddDbContext<SuggestionsAppContext>(options => options.UseSqlServer(connectionString));

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

app.UseEndpoints(endpoint =>
{
    endpoint.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
    
    endpoint.MapRazorPages();
});

app.Run();
