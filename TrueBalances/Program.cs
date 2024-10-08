using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using TrueBalances.Data;
using TrueBalances.Models;
using TrueBalances.Repositories;
using TrueBalances.Repositories.Interfaces;
using TrueBalances.Services;
using TrueBalances.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DbContextConnection") ?? throw new InvalidOperationException("Connection string 'DbContextConnection' not found.");

builder.Services.AddDbContext<TrueBalances.Data.TrueBalancesDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<CustomUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<TrueBalances.Data.TrueBalancesDbContext>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IProfilePhotoService, ProfilePhotoService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserGroupRepository, UserGroupRepository>();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<TrueBalances.Data.TrueBalancesDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbContextConnection"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// Ajouter cette ligne pour gérer les pages 404 et autres codes d'erreur.
app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Configuration du middleware de localisation pour définir la culture par défaut (français de France) 
// et les cultures supportées pour l'application.
var supportedCultures = new[]
{
    new CultureInfo("fr-FR")
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("fr-FR"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.Run();
