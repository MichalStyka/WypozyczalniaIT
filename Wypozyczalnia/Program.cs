using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Wypozyczalnia.Data;
using Wypozyczalnia.Models;


var builder = WebApplication.CreateBuilder(args);


//dodanie tego do wysy�ania maila
builder.Services.AddTransient<IEmailSender, EmailSender>();



var defaultDateCulture = "pl-PL";
var ci = new CultureInfo(defaultDateCulture);
ci.NumberFormat.NumberDecimalSeparator = ".";
ci.NumberFormat.CurrencyDecimalSeparator = ".";




// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
//    .AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();



builder.Services.AddControllersWithViews();

var app = builder.Build();

//

// Configure the Localization middleware
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(ci),
    SupportedCultures = new List<CultureInfo>
    {
        ci,
    },
    SupportedUICultures = new List<CultureInfo>
    {
        ci,
    }
});



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();



//Tworzenie rol i  sprawdzanie czy juz istnieja i tworzenie superadmina jak go nie ma


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "User" };

    foreach (var role in roles)
    {
        //sprawdza czy istnieje jak nie to tworzy
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }


}

//tworzenie SUPERADMINA i sprawdzanie czy jest jak nie to go tworzy


using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string email = "admin1@admin.com";
    string password = "Test1234!";


    //sprawdzanie czy jest

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new IdentityUser();
        user.UserName = email;
        user.Email = email;
        await userManager.CreateAsync(user, password);

        //dodawnie roli i jest tam await bo jest tam async
        await userManager.AddToRoleAsync(user, "Admin");
    }


}


app.Run();
