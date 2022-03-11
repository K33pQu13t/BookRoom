using BookingAudience.DTO.Users;
using BookingAudience.Enums;
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
    public class UserAdministratingService
    {
        private ClaimsPrincipal User
        {
            get
            {
                return _httpContextAccessor.HttpContext.User;
            }
        }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAdministratingService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// регистрация пользователя в система
        /// </summary>
        /// <param name="registerInfo"></param>
        /// <param name="errorMessage"></param>
        /// <returns>true при успешной регистрации</returns>
        public async Task RegisterAsync(RegisterDTO registerInfo, UserManager<AppUser> userManager)
        {
            //if (User.IsInRole(Role.Administrator.ToString()))
            //{
                //if (registerInfo.DateOfStartWorking.Date > DateTime.Now.Date)
                //    throw new StartWorkingDayException();

                var user = new AppUser()
                {
                    UserRole = registerInfo.UserRole,
                    FirstName = registerInfo.FirstName,
                    SecondName = registerInfo.SecondName,
                };

                var result = await userManager.CreateAsync(user, registerInfo.Password);
                if (result.Succeeded)
                {
                    await userManager.AddClaimAsync(
                        user, new Claim(ClaimTypes.Role, registerInfo.UserRole.ToString()));
                }
                else
                {
                    throw new Exception(string.Join(',', result.Errors));
                }
            //}
            //else
            //    throw new UserRoleException("регистрировать пользователей");
        }

        //public async Task EditUserAsync(EditDTO editInfo)
        //{
        //    IEditableUser userForEdit = await userManager.FindByIdAsync(editInfo.UserId.ToString());
        //    if (userForEdit == null)
        //        throw new UserNotFoundException();
        //    userForEdit.Name = editInfo.Name;
        //    userForEdit.Surname = editInfo.Surname;
        //    userForEdit.Patronymic = editInfo.Patronymic;
        //    userForEdit.Position = editInfo.Position;
        //    userForEdit.PhoneNumber = FormatPhoneNumber(editInfo.PhoneNumber);
        //    userForEdit.DateOfStartWorking = editInfo.DateOfStartWorking;

        //    if (userForEdit.UserRole != editInfo.UserRole)
        //    {
        //        userForEdit.UserRole = editInfo.UserRole;
        //        var userClaims = await userManager.GetClaimsAsync((User)userForEdit);
        //        Claim claimToReplace = userClaims.ToList().FirstOrDefault(c => c.Type == ClaimTypes.Role);
        //        await userManager.ReplaceClaimAsync(
        //            (User)userForEdit, claimToReplace, new Claim(ClaimTypes.Role, editInfo.UserRole.ToString()));
        //    }
        //    if (!string.IsNullOrEmpty(editInfo.Password))
        //    {
        //        var token = await userManager.GeneratePasswordResetTokenAsync((User)userForEdit);
        //        var result = await userManager.ResetPasswordAsync((User)userForEdit, token, editInfo.Password);
        //        if (!result.Succeeded)
        //        {
        //            throw new Exception(string.Join(',', result.Errors));
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(editInfo.Email))
        //    {
        //        var token = await userManager.GenerateChangeEmailTokenAsync((User)userForEdit, editInfo.Email);
        //        var result = await userManager.ChangeEmailAsync((User)userForEdit, editInfo.Email, token);
        //        if (!result.Succeeded)
        //        {
        //            throw new Exception(string.Join(',', result.Errors));
        //        }
        //    }

        //    usersRepository.Update((User)userForEdit);
        //}

        //public async Task DeleteUser(int userId)
        //{
        //    if (User.IsInRole(Role.Administrator.ToString()))
        //        await usersRepository.DeleteAsync(userId);
        //    else
        //        throw new UserRoleException("удалять пользователей");
        //}
    }
}
