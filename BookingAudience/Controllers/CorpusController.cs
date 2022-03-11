using BookingAudience.DAL.Repositories;
using BookingAudience.Models;
using BookingAudience.Models.Users;
using BookingAudience.Services.Audiences;
using BookingAudience.ViewModels;
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
        private readonly CorpusManagementService _corpusManagementService;

        public CorpusController(IHttpContextAccessor context, IServiceProvider provider)
        {
            _context = context;

            _usersRepository = (IGenericRepository<AppUser>)provider.GetService(typeof(IGenericRepository<AppUser>));
            _bookingsRepository = (IGenericRepository<Booking>)provider.GetService(typeof(IGenericRepository<Booking>));
            _audiencesRepository = (IGenericRepository<Audience>)provider.GetService(typeof(IGenericRepository<Audience>));
            _buildingsRepository = (IGenericRepository<Building>)provider.GetService(typeof(IGenericRepository<Building>));

            _corpusManagementService = new CorpusManagementService(_audiencesRepository, _buildingsRepository);

            //todo отладка, моковые данные
            var building1 = new Building() { Title = "Корпус Л", Address = "Где-то на левом", CodeLetter = 'Л' };
            var building2 = new Building() { Title = "Корпус А", Address = "Где-то у ракеты", CodeLetter = 'А' };
            _corpusManagementService.PushBuildingAsync(building1).GetAwaiter().GetResult();
            _corpusManagementService.PushBuildingAsync(building2).GetAwaiter().GetResult();

            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building1, Floor = 1, Number = 104 }).GetAwaiter().GetResult();

            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building2, Floor = 4, Number = 408 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building2, Floor = 1, Number = 104 }).GetAwaiter().GetResult();

            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building1, Floor = 3, Number = 311 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building1, Floor = 1, Number = 107 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building1, Floor = 3, Number = 333 }).GetAwaiter().GetResult();

            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building2, Floor = 3, Number = 322 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building2, Floor = 1, Number = 117 }).GetAwaiter().GetResult();

            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building1, Floor = 2, Number = 205 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building1, Floor = 1, Number = 128 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building1, Floor = 2, Number = 213 }).GetAwaiter().GetResult();

            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building2, Floor = 4, Number = 408 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building2, Floor = 3, Number = 310 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building2, Floor = 2, Number = 201 }).GetAwaiter().GetResult();
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("{controller}/Audiences")]
        public IActionResult GetAudiences()
        {
            var audiences = _corpusManagementService.GetAllAudiences();
            audiences = audiences.OrderBy(a => a.Building.Title).ToList();
            List<string> buildingTitles = audiences.Select(o => o.Building.Title).Distinct().ToList();

            List<List<Audience>> result = new List<List<Audience>>();
            for (int i = 0; i < buildingTitles.Count; i++)
            {
                result.Add(audiences.Where(a => a.Building.Title == buildingTitles[i]).OrderBy(a => a.Floor).ToList());
            }

            return View("Audiences", new AllAudiencesViewModel()
            { 
                Audiences = result
            });
        }

        public async Task<IActionResult> GetAudience(int id)
        {
            var audience = await _corpusManagementService.GetAudienceAsync(id);
            return View("Audience", new AudienceViewModel() { Building = audience.Building, Floor = audience.Floor, Number = audience.Number });
        }

        public async Task<IActionResult> GetBuilding(int id)
        {
            var building = await _corpusManagementService.GetBuildingAsync(id);
            return View("Audience", new BuildingViewModel() { Title = building.Title, Address = building.Address, CodeLetter = building.CodeLetter });
        }

        public async Task<IActionResult> AddAudience(int floor, int buildingId, int number)
        {
            var building = await _buildingsRepository.GetAsync(buildingId);
            await _corpusManagementService.PushAudienceAsync(new Audience() { Building = building, Floor = floor, Number = number });
            return View();
        }

        public async Task AddBuilding(string title, string address, char codeLetter)
        {
            await _corpusManagementService.PushBuildingAsync(new Building() { Title = title, Address = address, CodeLetter = codeLetter });
        }
    }
}