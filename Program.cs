using Lesson_22_Pizza_Star.Data;
using Lesson_22_Pizza_Star.Data.Helpers;
using Lesson_22_Pizza_Star.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pizza_Star.Interfaces;
using Pizza_Star.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore
// Install-Package Microsoft.EntityFrameworkCore.SqlServer 



IConfigurationRoot _confString = new ConfigurationBuilder().
    SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();

builder.Services.AddDbContext<ApplicationContext>(options =>
        options.UseSqlServer(_confString.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<User, IdentityRole>(opts =>
{
    opts.Password.RequiredLength = 3;   // ����������� �����
    opts.Password.RequireNonAlphanumeric = false;   // ��������� �� �� ���������-�������� �������
    opts.Password.RequireLowercase = false; // ��������� �� ������� � ������ ��������
    opts.Password.RequireUppercase = false; // ��������� �� ������� � ������� ��������
    opts.Password.RequireDigit = false; // ��������� �� �����


})
    .AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();


//������� ������������ ����� � ���������� � �������
var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig!);


//���������� ������ ��� �������� �����
builder.Services.AddScoped<EmailSender>();

// ���������� ����������� ��������� � �������...
builder.Services.AddScoped<ICategory, CategoryRepository>();

//����� ������������� ������, ��� �������������� ������ - 1 ���.
builder.Services.Configure<DataProtectionTokenProviderOptions>(opts => opts.TokenLifespan = TimeSpan.FromHours(1));


var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await DbInit.InitializeAsync(userManager, rolesManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}






app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
