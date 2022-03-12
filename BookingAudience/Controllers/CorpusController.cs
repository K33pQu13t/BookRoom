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
    [Authorize]
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

<<<<<<< HEAD
            var building = new Building() { Title = "Корпус Л", Address = "Где-то в Красноярске", CodeLetter = 'Л' };
            //то что ниже расклонировать
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building, Floor = 1, Number = 101 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building, Floor = 1, Number = 102 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building, Floor = 1, Number = 103 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building, Floor = 1, Number = 104 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building, Floor = 1, Number = 105 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building, Floor = 1, Number = 106 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building, Floor = 1, Number = 107 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building, Floor = 1, Number = 108 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building, Floor = 1, Number = 109 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building, Floor = 1, Number = 110 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building, Floor = 1, Number = 111 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building, Floor = 1, Number = 112 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building, Floor = 1, Number = 113 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building, Floor = 1, Number = 114 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building, Floor = 1, Number = 115 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building, Floor = 1, Number = 116 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building, Floor = 1, Number = 117 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building, Floor = 1, Number = 118 }).GetAwaiter().GetResult();
            _corpusManagementService.PushAudienceAsync(new Audience() { Building = building, Floor = 1, Number = 119 }).GetAwaiter().GetResult();
=======
            //todo отладка, моковые данные
            //var building1 = new Building() { Title = "Корпус Л", Address = "Где-то на левом", CodeLetter = 'л' };
            //var building2 = new Building() { Title = "Корпус А", Address = "Где-то у ракеты", CodeLetter = 'а' };
            //_corpusManagementService.PushBuildingAsync(building1).GetAwaiter().GetResult();
            //_corpusManagementService.PushBuildingAsync(building2).GetAwaiter().GetResult();

            //_corpusManagementService.PushAudienceAsync(new Audience() { Building = building1, Floor = 1, Number = 104 }).GetAwaiter().GetResult();

            //_corpusManagementService.PushAudienceAsync(new Audience() { Building = building2, Floor = 4, Number = 408 }).GetAwaiter().GetResult();
            //_corpusManagementService.PushAudienceAsync(new Audience() { Building = building2, Floor = 1, Number = 104 }).GetAwaiter().GetResult();

            //_corpusManagementService.PushAudienceAsync(new Audience() { Building = building1, Floor = 3, Number = 311 }).GetAwaiter().GetResult();
            //_corpusManagementService.PushAudienceAsync(new Audience() { Building = building1, Floor = 1, Number = 107 }).GetAwaiter().GetResult();
            //_corpusManagementService.PushAudienceAsync(new Audience() { Building = building1, Floor = 3, Number = 333 }).GetAwaiter().GetResult();

            //_corpusManagementService.PushAudienceAsync(new Audience() { Building = building2, Floor = 3, Number = 322 }).GetAwaiter().GetResult();
            //_corpusManagementService.PushAudienceAsync(new Audience() { Building = building2, Floor = 1, Number = 117 }).GetAwaiter().GetResult();

            //_corpusManagementService.PushAudienceAsync(new Audience() { Building = building1, Floor = 2, Number = 205 }).GetAwaiter().GetResult();
            //_corpusManagementService.PushAudienceAsync(new Audience() { Building = building1, Floor = 1, Number = 128 }).GetAwaiter().GetResult();
            //_corpusManagementService.PushAudienceAsync(new Audience() { Building = building1, Floor = 2, Number = 213 }).GetAwaiter().GetResult();

            //_corpusManagementService.PushAudienceAsync(new Audience() { Building = building2, Floor = 4, Number = 408 }).GetAwaiter().GetResult();
            //_corpusManagementService.PushAudienceAsync(new Audience() { Building = building2, Floor = 3, Number = 310 }).GetAwaiter().GetResult();
            //_corpusManagementService.PushAudienceAsync(new Audience() { Building = building2, Floor = 2, Number = 201 }).GetAwaiter().GetResult();
>>>>>>> 5cef51ba61da00bf53f82198b395f3f6d0f0b7a5
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/audiences")]
        public IActionResult GetAudiences()
        {
            var result = _corpusManagementService.GetAllAudiencesSortedByBuildingAndNumber();

            return View("Audiences", new AllAudiencesViewModel()
            { 
                ListOfListsOfAudiences = result
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