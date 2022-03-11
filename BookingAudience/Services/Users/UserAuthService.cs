using BookingAudience.DTO.Users;
using BookingAudience.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookingAudience.Services.Users
{
    public class UserAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// id текущего пользователя
        /// </summary>
        public int CurrentUserId
        {
            get
            {
                return int.Parse(
                    _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
        }

        public UserAuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task LogInAsync(LoginDTO loginInfo, 
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            //User user = usersRepository.Get().FirstOrDefault(u =>
            //    u.Email == loginInfo.Email &&
            //    u.Password == loginInfo.Password);
            //if (user == null)
            //{
            //    return false;
            //}

            var user = await userManager.FindByEmailAsync(loginInfo.Email);
            if (user == null)
            {
                throw new Exception("Неверный логин пароль");
            }
            var result = await signInManager.PasswordSignInAsync(
                user,
                loginInfo.Password,
                true,
                false);
            if (!result.Succeeded)
            {
                
                throw new Exception("Неверный логин пароль");
            }
        }

        public async Task LogOffAsync(SignInManager<AppUser> signInManager)
        {
            await signInManager.SignOutAsync();
        }
    }
}
