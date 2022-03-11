using BookingAudience.DAL.Repositories;
using BookingAudience.Models;
using BookingAudience.Models.Users;
using BookingAudience.Services.Audiences;
using BookingAudience.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.Controllers
{
    //[Authorize]
    public class CorpusController : Controller
    {
        private readonly IHttpContextAccessor _context;
        private readonly IGenericRepository<AppUser> _usersRepository;
        private readonly IGenericRepository<Booking> _bookingsRepository;
        private readonly IGenericRepository<Audience> _audiencesRepository;
        private readonly IGenericRepository<Building> _buildingsRepository;
        private readonly CorpusManagementService _audiencesManagementService;

        public CorpusController(IHttpContextAccessor context, IServiceProvider provider)
        {
            _context = context;

            _usersRepository = (IGenericRepository<AppUser>)provider.GetService(typeof(IGenericRepository<AppUser>));
            _bookingsRepository = (IGenericRepository<Booking>)provider.GetService(typeof(IGenericRepository<Booking>));
            _audiencesRepository = (IGenericRepository<Audience>)provider.GetService(typeof(IGenericRepository<Audience>));

            _audiencesManagementService = new CorpusManagementService(_audiencesRepository, _buildingsRepository);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAudiences()
        {
            return View(new AllAudiencesViewModel() 
            { 
                
            });
        }

        public async Task<IActionResult> GetAudience(int id)
        {
            var audience = await _audiencesManagementService.GetAudienceAsync(id);
            return View("Audience", new AudienceViewModel() { Building = audience.Building, Floor = audience.Floor, Number = audience.Number });
        }

        public async Task<IActionResult> GetBuilding(int id)
        {
            var building = await _audiencesManagementService.GetBuildingAsync(id);
            return View("Audience", new BuildingViewModel() { Title = building.Title, Address = building.Address, CodeLetter = building.CodeLetter });
        }

        public async Task<IActionResult> AddAudience(int floor, int buildingId, int number)
        {
            var building = await _buildingsRepository.GetAsync(buildingId);
            await _audiencesManagementService.PushAudienceAsync(new Audience() { Building = building, Floor = floor, Number = number });
            return View();
        }

        public async Task<IActionResult> AddBuilding(string title, string address, char codeLetter)
        {
            await _audiencesManagementService.PushBuildingAsync(new Building() { Title = title, Address = address, CodeLetter = codeLetter });
            return View();
        }
    }
}