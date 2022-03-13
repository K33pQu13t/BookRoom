using BookingAudience.DAL.Repositories;
using BookingAudience.Models;
using BookingAudience.Models.Users;
using BookingAudience.Services.Bookings;
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
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IHttpContextAccessor _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        private readonly UserManagementService _userManagmentService;
        private readonly UserAuthService _userAuthService;
        private readonly BookingManagementService _bookingManagementService;
        private readonly IGenericRepository<Booking> _bookingRepository;


        public BookingController(IHttpContextAccessor context,
            IServiceProvider provider,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;

            var usersRepository = (IGenericRepository<AppUser>)provider.GetService(typeof(IGenericRepository<AppUser>));
            _bookingRepository = (IGenericRepository<Booking>)provider.GetService(typeof(IGenericRepository<Booking>));
            _userAuthService = new UserAuthService(context);
            _userManagmentService = new UserManagementService(
                usersRepository,
                _userAuthService,
                userManager);
            _bookingManagementService = new BookingManagementService(usersRepository, _bookingRepository, _userAuthService, userManager);
        }

        public async Task<IActionResult> Book(BookingViewModel bookingViewModel)
        {
            Booking booking = new Booking()
            {
                BookedAudience = bookingViewModel.BookedAudience,
                BookingTime = bookingViewModel.BookingTime,
                DurationInMinutes = bookingViewModel.DurationInMinutes,
                Creator = bookingViewModel.Creator
            };
            await _bookingManagementService.BookAudienceAsync(_bookingRepository, booking);
            return View();
        }

       
    }
}
