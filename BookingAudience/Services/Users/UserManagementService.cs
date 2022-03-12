using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingAudience.DAL;
using BookingAudience.Models.Users;
using BookingAudience.DAL.Repositories;
using BookingAudience.Enums;
using BookingAudience.DTO.Users;
using System.Security.Claims;
using System;

namespace BookingAudience.Services.Users
{
    /// <summary>
    /// класс, описывающий логику взаимодействия с пользователями (регистрация, логин и тд.)
    /// </summary>
    public class UserManagementService
    {
        private readonly IGenericRepository<AppUser> usersRepository;
        private readonly UserAuthService userAuthService;
        private readonly UserManager<AppUser> userManager;

        public int CurrentUserId
        {
            get
            {
                return userAuthService.CurrentUserId;
            }
        }
        public UserManagementService(
            IGenericRepository<AppUser> usersRepository,
            UserAuthService userAuthService,
            UserManager<AppUser> userManager)
        {
            this.usersRepository = usersRepository;
            this.userAuthService = userAuthService;
            this.userManager = userManager;
        }

        /// <summary>
        /// получить пользователя из бд
        /// </summary>
        /// <param name="userId">идентификатор пользователя. Null если текущий</param>
        /// <returns></returns>
        public async Task<AppUser> GetUserAsync(int? userId = default)
        {
            AppUser user;
            //если айди не задан, то достаём текущего пользователя
            if (userId == null)
            {
                user = await usersRepository.GetAsync(userAuthService.CurrentUserId);
            }
            //иначе ищем пользователя по переданному айди
            else
            {
                user = await usersRepository.GetAsync((int)userId);
                //todo
                //if (user == null)
                //    throw new UserNotFoundException();
            }
            return user;
        }

        /// <summary>
        /// получить пользователя для просмотра
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //public async Task<AppUser> GetUserToShowAsync(int? userId) 
        //{
        //    var user = await GetUserAsync(userId);

        //    //если получили не текущего пользователя
        //    if (user.Id != CurrentUserId)
        //    {
        //        //если найденный юзер - сотрудник
        //        if (user.UserRole == Role.Employee)
        //        {
        //            //а тот кто хочет открыть не руководитель, не директор и не админ, то не даём
        //            if (!userAuthService.User.IsInRole(Role.Supervisor.ToString()) &&
        //                !userAuthService.User.IsInRole(Role.Director.ToString()) &&
        //                !userAuthService.User.IsInRole(Role.Administrator.ToString()) &&
        //                user.Id != userAuthService.CurrentUserId)
        //                throw new UserRoleException("просматривать страницы других пользователей");
        //        }
        //        //а если это не сотрудник, а кто-то другой, тогда его получить могут только директор или админ
        //        else if (!userAuthService.User.IsInRole(Role.Director.ToString()) &&
        //                 !userAuthService.User.IsInRole(Role.Administrator.ToString()))
        //        {
        //            throw new UserRoleException("просматривать страницы других пользователей");
        //        }   
        //    }
        //    return user;
        //}

        /// <summary>
        /// получить всех пользователей из бд
        /// </summary>
        /// <param name="currentUserIncluded">true если в списке должен быть текущий пользователь</param>
        /// <returns></returns>
        public List<AppUser> GetUsers(bool currentUserIncluded = true)
        {
            if (currentUserIncluded)
                return usersRepository.Get().ToList();
            else
                return usersRepository.Get().Where(u => u.Id != userAuthService.CurrentUserId).ToList();
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
                LastName = registerInfo.SecondName,
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

        /// <summary>
        /// получить список пользователей, которые могут рассмотреть утверждение заявки (Руководители и Директоры)
        /// </summary>
        /// <returns></returns>
        //public List<User> GetSupervisors()
        //{
        //    return usersRepository.Get().Where(u => u.UserRole == Role.Supervisor || u.UserRole == Role.Director).ToList();
        //}

        /// <summary>
        /// скастить список пользователей в список объектов SelectListItem
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        //public List<SelectListItem> CastListOfUsersToListOfSelectListItems(List<User> users)
        //{
        //    List<SelectListItem> userItems = new List<SelectListItem>();
        //    for(int i = 0; i < users.Count; i++)
        //    {
        //        userItems.Add(new SelectListItem(users[i].InitialsWithPositionAndRole, users[i].Id.ToString()));
        //    }
        //    return userItems;
        //}

        /// <summary>
        /// получить список пользователей с того же подразделения, откуда текущий юзер
        /// </summary>
        /// <returns></returns>
        //public async Task<List<SelectListItem>> CastListOfUsersToListOfSelectListItemsForRequestCreation()
        //{
        //    var currentUser = await GetUserAsync();
        //    //оставляем только тех кто в одном отделении с текущим юзером
        //    var users = GetUsers(false).Where(u => u.Position == currentUser.Position).ToList();
        //    List<SelectListItem> userItems = new List<SelectListItem>();
        //    for (int i = 0; i < users.Count; i++)
        //    {
        //        userItems.Add(new SelectListItem(users[i].InitialsWithPositionAndRole, users[i].Id.ToString()));
        //    }
        //    return userItems;
        //}

        /// <summary>
        /// получить список руководителей с того же подразделения, откуда текущий юзер
        /// </summary>
        /// <returns></returns>
        //public async Task<List<SelectListItem>> CastListOfSupervisorsToListOfSelectListItemsForRequestCreation()
        //{
        //    var currentUser = await GetUserAsync();
        //    //оставляем только тех кто в одном отделении с текущим юзером
        //    var users = GetSupervisors().Where(u => u.Position == currentUser.Position).ToList();
        //    List<SelectListItem> userItems = new List<SelectListItem>();
        //    for (int i = 0; i < users.Count; i++)
        //    {
        //        userItems.Add(new SelectListItem(users[i].InitialsWithPositionAndRole, users[i].Id.ToString()));
        //    }
        //    return userItems;
        //}

        /// <summary>
        /// формирует список пользователей, страницы которых текущий пользователь может просматривать. Выбранный пользователь будет в начале списка
        /// </summary>
        /// <param name="userId">идентификатор пользователя. Null если текущий залогиненный пользователь</param>
        /// <returns></returns>
        //public async Task<List<SelectListItem>> GetUsersSelectListItemsForUserPageAsync(int? userId = null)
        //{
        //    User user;
        //    if (userId == null)
        //        user = await usersRepository.GetAsync(userAuthService.CurrentUserId);
        //    else
        //        user = await usersRepository.GetAsync((int)userId);

        //    if (user == null)
        //        throw new UserNotFoundException();

        //    //только эти роли могут просматривать страницы других пользователей
        //    if (userAuthService.User.IsInRole(Role.Administrator.ToString()) ||
        //        userAuthService.User.IsInRole(Role.Director.ToString()) ||
        //        userAuthService.User.IsInRole(Role.Supervisor.ToString()))
        //    {
        //        //составить список для drop down меню с выбором пользователей
        //        List<SelectListItem> userItems = new List<SelectListItem>();
        //        List<User> users = usersRepository.Get().ToList();
        //        if (userAuthService.User.IsInRole(Role.Supervisor.ToString()))
        //        {
        //            //если текущий юзер руководитель, то он может только сотрудников смотреть
        //            users = users.Where(u => u.UserRole == Role.Employee).ToList();
        //        }

        //        users.Remove(user);
        //        //если залогиненный пользователь находится на другой странице, не показываем ему в списке его страницу
        //        users.RemoveAll(us => us.Id == userAuthService.CurrentUserId);
        //        //добавляем в список остальных пользователей
        //        userItems = CastListOfUsersToListOfSelectListItems(users);
        //        //поместить пользователя которого смотрим на первое место в списке
        //        //если это залогиненный юзер, то вместо имени пишем "Вы"
        //        if (userAuthService.CurrentUserId == user.Id)
        //            userItems.Insert(0, new SelectListItem("Вы", userAuthService.CurrentUserId.ToString(), true));
        //        else
        //            userItems.Insert(0, new SelectListItem(user.InitialsWithPositionAndRole, user.Id.ToString(), true));
        //        return userItems;
        //    }
        //    return null;
        //    //throw new UserRoleException("просматривать страницы других пользователей");
        //}

        /// <param name="userId">идентификатор пользователя. Null если текущий пользователь</param>
        /// <returns>true если указаный пользователь - администратор</returns>
        public async Task<bool> IsAdministratorAsync(int? userId = null)
        {
            if (userId == null)
                userId = userAuthService.CurrentUserId;
            return (await GetUserAsync(userId)).UserRole == Role.Administrator;
        }

        /// <param name="userId">идентификатор пользователя. Null если текущий пользователь</param>
        /// <returns>true если указаный пользователь - руководитель</returns>
        //public async Task<bool> IsSupervisorAsync(int? userId = null)
        //{
        //    if (userId == null)
        //        userId = userAuthService.CurrentUserId;
        //    return (await GetUserAsync(userId)).UserRole == Role.Supervisor;
        //}

        /// <param name="userId">идентификатор пользователя. Null если текущий пользователь</param>
        /// <returns>true если указаный пользователь - директор</returns>
        //    public async Task<bool> IsDirectorAsync(int? userId = null)
        //    {
        //        if (userId == null)
        //            userId = userAuthService.CurrentUserId;
        //        return (await GetUserAsync(userId)).UserRole == Role.Director;
        //    }
    }
}
