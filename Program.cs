using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OstaFeedbackApp.Data;

var builder = WebApplication.CreateBuilder(args);

// =====================
// DATABASE (PostgreSQL)
// =====================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// =====================
// IDENTITY (AUTH SYSTEM)
// =====================
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    // Optional password settings
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddRoles<IdentityRole>() // IMPORTANT for roles
.AddEntityFrameworkStores<AppDbContext>();

// =====================
// MVC
// =====================
builder.Services.AddControllersWithViews();

var app = builder.Build();

// =====================
// CREATE ADMIN ROLE + USER
// =====================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    string adminEmail = "admin@osta.com";
    string adminPassword = "Admin@123";

    // Create Admin Role
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // Create Admin User
    var user = await userManager.FindByEmailAsync(adminEmail);
    if (user == null)
    {
        user = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(user, adminPassword);
    }

    // Assign Admin Role
    if (!await userManager.IsInRoleAsync(user, "Admin"))
    {
        await userManager.AddToRoleAsync(user, "Admin");
    }
}

// =====================
// MIDDLEWARE
// =====================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 🔐 VERY IMPORTANT
app.UseAuthentication();
app.UseAuthorization();

// =====================
// ROUTING
// =====================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Feedback}/{action=Create}/{id?}");

// Identity pages (login/register)
app.MapRazorPages();

app.Run();