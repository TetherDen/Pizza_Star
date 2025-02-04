using Lesson_22_Pizza_Star.Models;
using Microsoft.AspNetCore.Identity;

namespace Lesson_22_Pizza_Star.Data
{
    public class DbInit
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (await roleManager.FindByNameAsync("Editor") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Editor"));
            }
            if (await roleManager.FindByNameAsync("Client") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Client"));
            }


            string adminEmail = "admin@gmail.com", adminPassword = "123";
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User
                {
                    Email = adminEmail,
                    UserName = adminEmail,      // <<--- Авторизация по  етому полю
                    PhoneNumber = "380970601478",
                    Year = 1990,
                    City = "Днепр",
                    Address = "Титова 12, кв 33."
                };
                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }


                User alex = new User
                {
                    Email = "alex@gmail.com",
                    UserName = "alex@gmail.com",
                    PhoneNumber = "38096546798",
                    Year = 2001,
                    City = "Днепр",
                    Address = "Карла Маркса 121, кв 32."
                };
                result = await userManager.CreateAsync(alex, "qwerty");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(alex, "Editor");
                }


                User tom = new User
                {
                    Email = "tom@gmail.com",
                    UserName = "tom@gmail.com",
                    PhoneNumber = "380665459874",
                    Year = 1995,
                    City = "Днепр",
                    Address = "Тополь 3, дом 44 кв 7."
                };
                result = await userManager.CreateAsync(tom, "1234567AS");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(tom, "Client");
                }


                // PS мое: в контроолере в действии Login, оно на самом деле при авторизации првоеряет не по имейлу юзера а по его UserName
                // там надо изменить логику и сначало в юзерменеджере найти юзера по имейлу и если он не нулл передать в сигнменеджер обьект юзера...
                User marry = new User
                {
                    Email = "123@gmail.com",
                    UserName = "123@gmail.com",    // <<--- Авторизация по  етому полю
                    PhoneNumber = "38096123",
                    Year = 1981,
                    City = "Киев",
                    Address = "Шевеченко, дом 7 кв 14."
                };
                result = await userManager.CreateAsync(marry, "123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(marry, "Client");
                }
            }
        }
    }





}
