using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OstaFeedbackApp.Data;

var builder = WebApplication.CreateBuilder(args);

// =====================
// DATABASE (PostgreSQL)
// =====================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(5); // 🔥 retry if DB not ready
        }));

// =====================
// IDENTITY (AUTH SYSTEM)
// =====================
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    // Password settings (can tighten later)
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();

// =====================
// MVC + Razor Pages
// =====================
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// =====================
// AUTO DATABASE MIGRATION
// =====================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync(); // 🔥 auto create/update DB

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

            var result = await userManager.CreateAsync(user, adminPassword);

            if (!result.Succeeded)
            {
                throw new Exception("Failed to create admin user: " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        // Assign Admin Role
        if (!await userManager.IsInRoleAsync(user, "Admin"))
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("🔥 Startup Error: " + ex.Message);
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

// 🔐 AUTH
app.UseAuthentication();
app.UseAuthorization();

// =====================
// ROUTING
// =====================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Feedback}/{action=Create}/{id?}");

app.MapRazorPages();

app.Run();
