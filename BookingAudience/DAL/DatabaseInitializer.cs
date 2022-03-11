using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using BookingAudience.Enums;
using BookingAudience.Models.Users;

namespace BookingAudience.DAL
{
    /// <summary>
    /// создаёт корневого админа, если его нет в базе
    /// </summary>
    public class DatabaseInitializer
    {
        public static void Init(IServiceProvider serviceProvider)
        {
            UserManager<AppUser> userManager = 
                (UserManager<AppUser>)serviceProvider.GetService(typeof(UserManager<AppUser>));

            //создаём админа поумолчанию
            var tmpAdmin = new AppUser()
            {
                FirstName = "Геннадий",
                LastName = "Букин",
                UserRole = Role.Administrator,
                Email = "adm@mail.ru",
                UserName = "root"
            };

            var result = userManager.CreateAsync(tmpAdmin, "qwer").GetAwaiter().GetResult();
            if (result.Succeeded)
            {
                userManager.AddClaimAsync(
                    tmpAdmin, new Claim(ClaimTypes.Role, nameof(Role.Administrator)))
                        .GetAwaiter().GetResult();
            }

        }
    }
}
