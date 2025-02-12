using Lesson_22_Pizza_Star.Data;
using Lesson_22_Pizza_Star.Data.Helpers;
using Lesson_22_Pizza_Star.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pizza_Star.Interfaces;
using Pizza_Star.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddMemoryCache();
builder.Services.AddSession();

// Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore
// Install-Package Microsoft.EntityFrameworkCore.SqlServer 



IConfigurationRoot _confString = new ConfigurationBuilder().
    SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();

builder.Services.AddDbContext<ApplicationContext>(options =>
        options.UseSqlServer(_confString.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<User, IdentityRole>(opts =>
{
    opts.Password.RequiredLength = 3;   // минимальная длина
    opts.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
    opts.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
    opts.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
    opts.Password.RequireDigit = false; // требуются ли цифры


})
    .AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();


//Парсинг конфигурации почты и добавление в сервисы
var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig!);


//Подключаем сервис для отправки почты
builder.Services.AddScoped<EmailSender>();

// подключаем репозиторий категорий в сервисы...
builder.Services.AddScoped<ICategory, CategoryRepository>();

//Время существования токена, для восстановления пароля - 1 час.
builder.Services.Configure<DataProtectionTokenProviderOptions>(opts => opts.TokenLifespan = TimeSpan.FromHours(1));

// Подключаем сервис для работы с блюдами
builder.Services.AddScoped<IProduct, ProductRepository>();


// Сервисы Корзины и заказа:
// Подключаем сервис для работы с корзиной
builder.Services.AddScoped(e => CartRepository.GetCart(e));
//Подключаем сервис для работы с заказами
builder.Services.AddTransient<IOrder, OrderRepository>();

//===========================

var app = builder.Build();

app.UseSession();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();


        //=====================================================================

        // Ининциализация базы данных тестовыми данными

        // PS после инициалзиии закоменти код, иначе  при каждом запуске  приложения будут обнуляется данные которые там не инициализиовраоны
        // такие как оценки товаров - удаляются в бд при перезапуске апликейшена...

        //await DbInit.InitializeAsync(userManager, rolesManager);

        //var applicationContext = services.GetRequiredService<ApplicationContext>();
        //await DbInit.InitializeContentAsync(applicationContext);

        //await DbInit.CreateSeedDataAsync(applicationContext, categories: new int[] { 1, 2, 3 });

        //=====================================================================
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
