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


//Seed Data
var scopedFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopedFactory.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // seed Roles
    await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
    await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
    await roleManager.CreateAsync(new IdentityRole(Roles.Writer.ToString()));
    await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));


    // seed Admin User
    var admin = new ApplicationUser
    {
        Email = "behnamasaei@gmail.com",
        UserName = "admin",
        EmailConfirmed = true,
        PhoneNumberConfirmed = true,
        LockoutEnabled = false,
    };

    if (userManager.Users.All(u => u.Id != admin.Id))
    {
        var user = await userManager.FindByEmailAsync(admin.Email);
        if (user == null)
        {
            await userManager.CreateAsync(admin, "123456789");
            await userManager.AddToRoleAsync(admin, Roles.Basic.ToString());
            await userManager.AddToRoleAsync(admin, Roles.Writer.ToString());
            await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
            await userManager.AddToRoleAsync(admin, Roles.SuperAdmin.ToString());
        }

    }

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
