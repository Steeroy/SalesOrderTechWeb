using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalesOrderTechTestWeb.Areas.Identity.Data;
using SalesOrderTechTestWeb.Data;
using SalesOrderTechTestWeb.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SalesOrderWebDbContextConnection") ?? throw new InvalidOperationException("Connection string 'SalesOrderTechTestWebDbContextConnection' not found.");

builder.Services.AddDbContext<SalesOrderTechTestWebDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<SalesOrderTechTestWebDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Order}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
