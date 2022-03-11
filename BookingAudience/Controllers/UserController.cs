using BookingAudience.DAL.Repositories;
using BookingAudience.Models.Users;
using BookingAudience.Services.Users;
using BookingAudience.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpContextAccessor _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        private readonly UserManagementService _userManagerService;
        private readonly UserAuthService _userAuthService;
        private readonly UserAdministratingService _userAdministratingService;

        public UserController(IHttpContextAccessor context,
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

        public IActionResult AdminPanel()
        {
            return View();
        }

        public IActionResult CreateBuilding()
        {
            return View("AdminBuilding", new BuildingViewModel());
        }

        public IActionResult CreateAudience()
        {
            return View("AdminAudience", new AudienceViewModel());
        }
        public IActionResult CreateUser()
        {
            return View("AdminUser", new UserViewModel());
        }
    }
}
