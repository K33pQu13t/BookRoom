using BookingAudience.DAL.Repositories;
using BookingAudience.DTO.Users;
using BookingAudience.Enums;
using BookingAudience.Models.Users;
using BookingAudience.Services.Users;
using BookingAudience.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.Controllers
{
    public class AuthoriseController : Controller
    {
        private readonly IHttpContextAccessor _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        private readonly UserManagementService _userManagerService;
        private readonly UserAuthService _userAuthService;
        private readonly UserAdministratingService _userAdministratingService;

        public AuthoriseController(IHttpContextAccessor context,
            IServiceProvider provider,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;

            var usersRepository = (IGenericRepository<AppUser>)provider.GetService(typeof(IGenericRepository<AppUser>));
            _userAuthService = new UserAuthService(context);
            _userManagerService = new UserManagementService(
                usersRepository,
                _userAuthService,
                userManager);
            _userAdministratingService = new UserAdministratingService(context);
        }

        public async Task<IActionResult> Index(int? userId = null)
        {
            //если передавали в адресе айдишник
            if (RouteData.Values.ContainsKey("id"))
            {
                userId = int.Parse(RouteData.Values["id"].ToString());
            }

            AppUser user = await _userManagerService.GetUserAsync(userId);
            //ViewBag.UserList = await _userManagerService.GetUsersSelectListItemsForUserPageAsync(userId);

            return View("Index",
                new UserViewModel()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    BirthDay = user.BirthDay
                });
        }

        [Authorize(Policy = nameof(Role.Administrator))]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    ViewBag.roles = roles;
            //    ViewBag.positions = positions;
            //    return View(model);
            //}

            try
            {
                await _userAdministratingService.RegisterAsync(
                new RegisterDTO()
                {
                    UserRole = model.UserRole,
                    FirstName = model.FirstName,
                    SecondName = model.SecondName,
                    Password = model.Password
                }, 
                _userManager);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _userAuthService.LogInAsync(
                    new LoginDTO()
                    {
                        Email = model.Login,
                        Password = model.Password
                    },
                    _userManager, _signInManager);
                return RedirectToAction("Success", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
    }
}
