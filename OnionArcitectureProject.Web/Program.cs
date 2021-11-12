using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnionArcitectureProject.DataLayer.Context;
using OnionArcitectureProject.Entities;
using OnionArcitectureProject.ProgramConfig;

var builder = WebApplication.CreateBuilder(args);


/// <summary>
/// ////////
/// </summary>
var connectionString = builder.Configuration
    .GetConnectionString("ApplicationDbContextConnection"); 

builder.Services.AddDbContext<ApplicationDbContext>(options =>
     options.UseSqlServer(connectionString));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

//builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
//    options.SignIn.RequireConfirmedAccount = false)
//      .AddEntityFrameworkStores<ApplicationDbContext>()
//      .AddDefaultTokenProviders();

// Ioc
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddCustomService();

// Add services to the container.
builder.Services.AddControllersWithViews();


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
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

    // add for identity
    endpoints.MapRazorPages();
});

app.Run();
