using BookingAudience.DAL.Repositories;
using BookingAudience.Enums;
using BookingAudience.Extensions;
using BookingAudience.Models;
using BookingAudience.Models.Users;
using BookingAudience.Services.Audiences;
using BookingAudience.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.Controllers
{
    [Authorize]
    public class CorpusController : Controller
    {
        private readonly IHttpContextAccessor _context;
        private readonly IGenericRepository<AppUser> _usersRepository;
        private readonly IGenericRepository<Booking> _bookingsRepository;
        private readonly IGenericRepository<Audience> _audiencesRepository;
        private readonly IGenericRepository<Building> _buildingsRepository;
        private readonly CorpusManagementService _corpusManagementService;

        readonly List<SelectListItem> audienceTypes = new List<SelectListItem>()
        {
            new SelectListItem(AudienceType.ClassRoom.GetDescription(), ((int)AudienceType.ClassRoom).ToString()),
            new SelectListItem(AudienceType.Audience.GetDescription(), ((int)AudienceType.Audience).ToString()),
            new SelectListItem(AudienceType.AssemblyHall.GetDescription(), ((int)AudienceType.AssemblyHall).ToString())
        };

        public CorpusController(IHttpContextAccessor context, IServiceProvider provider)
        {
            _context = context;

            _usersRepository = (IGenericRepository<AppUser>)provider.GetService(typeof(IGenericRepository<AppUser>));
            _bookingsRepository = (IGenericRepository<Booking>)provider.GetService(typeof(IGenericRepository<Booking>));
            _audiencesRepository = (IGenericRepository<Audience>)provider.GetService(typeof(IGenericRepository<Audience>));
            _buildingsRepository = (IGenericRepository<Building>)provider.GetService(typeof(IGenericRepository<Building>));

            _corpusManagementService = new CorpusManagementService(_audiencesRepository, _buildingsRepository);

            //todo отладка, моковые данные
            var building1 = new Building() { Title = "Корпус Л", Address = "Где-то на левом", CodeLetter = 'л' };
            var building2 = new Building() { Title = "Корпус А", Address = "Где-то у ракеты", CodeLetter = 'а' };
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

        [Route("/audiences")]
        public IActionResult GetAudiences(int buildingId = 0, int floor = 0, int type = 0)
        {
            var result = _corpusManagementService.GetAllAudiencesSortedByBuildingAndNumber(buildingId, floor);

            List<SelectListItem> buildings = new List<SelectListItem>();
            List<SelectListItem> floors = new List<SelectListItem>();

            List<int> uniqueFloors = new List<int>();
            for (int i = 0; i < result.Count; i++)
            {
                buildings.Add(new SelectListItem(result[i][0].Building.Title, result[i][0].Building.Id.ToString()));
                uniqueFloors.AddRange(result[i].Select(x => x.Floor).ToList());
            }
            uniqueFloors = uniqueFloors.Distinct().ToList();

            for (int i = 0; i < uniqueFloors.Count; i++)
            {
                floors.Add(new SelectListItem(uniqueFloors[i].ToString(), uniqueFloors[i].ToString()));
            }

            return View("Audiences", new AllAudiencesViewModel()
            { 
                ListOfListsOfAudiences = result,
                Buildings = buildings,
                Floors = floors,
                AudienceTypes = audienceTypes
            });
        }

        [Route("/audiences/{fullNumber}")]
        public IActionResult GetAudience(string fullNumber)
        {
            char codeLetter = fullNumber[0];
            int number = 0;
            if (!int.TryParse(fullNumber.Substring(1), out number))
            {
                throw new Exception("Неверный формат названия кабинета");
            }

            var audience = _corpusManagementService.GetAllAudiences().FirstOrDefault(a => a.Building.CodeLetter.ToString().ToLower() == codeLetter.ToString() && a.Number == number);
            if (audience == null)
                throw new Exception($"Аудитории \"{codeLetter}{number}\" не существует");
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
            //сделать букву маленькой
            codeLetter = codeLetter.ToString().ToLower().ToCharArray()[0];
            await _corpusManagementService.PushBuildingAsync(new Building() { Title = title, Address = address, CodeLetter = codeLetter });
        }
    }
}