using Microsoft.EntityFrameworkCore;
using SuggestionsApp.Models.Data.Database;
using SuggestionsApp.Models.Data.Identity;
using SuggestionsApp.Models.Interfaces;
using SuggestionsApp.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
var configutation = builder.Configuration;

var connectionString = configutation.GetConnectionString("SuggestionsApp");

// Identity DB Context
services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
// SuggestionsApp DB Context
services.AddDbContext<SuggestionsAppContext>(options => options.UseSqlServer(connectionString));

// Identity implementation and configuration
services.AddDefaultIdentity<ApplicationUser>(options => {
    options.SignIn.RequireConfirmedAccount = true;

    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;

}).AddEntityFrameworkStores<ApplicationContext>();

services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "Identity.Cookie";
    config.LoginPath = "/Account/Login";
});

services.AddHttpContextAccessor();
services.AddControllersWithViews().AddRazorRuntimeCompilation();
services.AddRazorPages();

// Dependency Injection - Services
services.AddTransient<ISuggestionsService, SuggestionsService>();
services.AddTransient<ICategoriesService, CategoriesService>();
services.AddTransient<IStatesService, StatesService>();
services.AddTransient<IUserService, UserService>();

// Depencency Injection - AutoMapper
services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


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
