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
    public class AuthorizeController : Controller
    {
        private readonly IHttpContextAccessor _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        private readonly UserManagementService _userManagmentService;
        private readonly UserAuthService _userAuthService;

        public AuthorizeController(IHttpContextAccessor context,
            IServiceProvider provider,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;

            var usersRepository = (IGenericRepository<AppUser>)provider.GetService(typeof(IGenericRepository<AppUser>));
            _userAuthService = new UserAuthService(context);
            _userManagmentService = new UserManagementService(
                usersRepository,
                _userAuthService,
                userManager);
        }

        public async Task<IActionResult> Index(int? userId = null)
        {
            //если передавали в адресе айдишник
            if (RouteData.Values.ContainsKey("id"))
            {
                userId = int.Parse(RouteData.Values["id"].ToString());
            }

            AppUser user = await _userManagmentService.GetUserAsync(userId);
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

            model.UserRole = Role.Student;
            try
            {
                await _userManagmentService.RegisterAsync(
                new RegisterDTO()
                {
                    UserRole = model.UserRole,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
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

        [Route("/login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginPost(LoginViewModel model)
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
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Login" ,model);
            }
        }

        [Route("/logout")]
        public async Task<IActionResult> Logout()
        {
            await _userAuthService.LogOutAsync(_signInManager);

            return RedirectToAction("Login");
            //return LocalRedirect("/Home/Index");
        }

        //private IActionResult SuccessLogin()
        //{
        //    return RedirectToAction("Index", "Home");
        //}
    }
}
