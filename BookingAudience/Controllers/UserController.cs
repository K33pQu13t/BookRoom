using BookingAudience.DAL.Repositories;
using BookingAudience.Enums;
using BookingAudience.Models;
using BookingAudience.Models.Users;
using BookingAudience.Services.Users;
using BookingAudience.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        private readonly IGenericRepository<Building> _buildingRepository;

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

            _buildingRepository = (IGenericRepository<Building>)provider.GetService(typeof(IGenericRepository<Building>));
        }

        [Authorize(Policy = nameof(Role.Administrator))]
        [Route("/admin")]
        public IActionResult AdminPanel()
        {
            List<Building> buildings = _buildingRepository.Get().ToList();
            List<SelectListItem> buildingsOptions = new();
            for(int i = 0; i < buildings.Count; i++)
            {
                buildingsOptions.Add(new SelectListItem(buildings[i].Title, buildings[i].Id.ToString()));
            }
            return View(new AdminPanelViewModel() 
            { 
                Buildings = buildings,
                BuildingsOptions = buildingsOptions
            });
        }

        [Authorize(Policy = nameof(Role.Administrator))]
        public IActionResult CreateBuilding()
        {
            return View("AdminBuilding", new BuildingViewModel());
        }

        [Authorize(Policy = nameof(Role.Administrator))]
        public IActionResult CreateAudience()
        {
            return View("AdminAudience", new AudienceViewModel());
        }

        [Authorize(Policy = nameof(Role.Administrator))]
        public IActionResult CreateUser()
        {
            return View("AdminUser", new UserViewModel());
        }
    }
}
