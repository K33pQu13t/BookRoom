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
using BookingAudience.Extensions;

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

        //readonly List<SelectListItem> audienceTypes = new List<SelectListItem>()
        //{
        //    new SelectListItem(AudienceType.ClassRoom.GetDescription(), ((int)AudienceType.ClassRoom).ToString()),
        //    new SelectListItem(AudienceType.Audience.GetDescription(), ((int)AudienceType.Audience).ToString()),
        //    new SelectListItem(AudienceType.AssemblyHall.GetDescription(), ((int)AudienceType.AssemblyHall).ToString())
        //};

        public CorpusController(IHttpContextAccessor context, IServiceProvider provider)
        {
            _context = context;

            _usersRepository = (IGenericRepository<AppUser>)provider.GetService(typeof(IGenericRepository<AppUser>));
            _bookingsRepository = (IGenericRepository<Booking>)provider.GetService(typeof(IGenericRepository<Booking>));
            _audiencesRepository = (IGenericRepository<Audience>)provider.GetService(typeof(IGenericRepository<Audience>));
            _buildingsRepository = (IGenericRepository<Building>)provider.GetService(typeof(IGenericRepository<Building>));

            _corpusManagementService = new CorpusManagementService(_audiencesRepository, _buildingsRepository);
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/audiences")]
        public IActionResult GetAudiences()
        {
            return View("Audiences", GetAllAudienceViewModel());
        }

        [Route("/audiences/filtered")]
        public IActionResult AudiencesUpdate(int buildingId = 0, int floor = 0, int type = 0)
        {
            return PartialView("Audiences", GetAllAudienceViewModel(buildingId, floor, type));
        }

        private AllAudiencesViewModel GetAllAudienceViewModel(int buildingId = 0, int floor = 0, int type = 0)
        {

            var result = _corpusManagementService.GetAllAudiencesSortedByBuildingAndNumber(buildingId, floor, type);
            if (result == null)
            {
                return new AllAudiencesViewModel();
            }

            //получаем все билдинги
            List<Building> buildingList = _corpusManagementService.GetAllBuildings();
            List<SelectListItem> buildings = new List<SelectListItem>()
            {
                new SelectListItem("Здание", "0")
            };
            for (int i = 0; i < buildingList.Count; i++)
            {
                buildings.Add(new SelectListItem(buildingList[i].Title, buildingList[i].Id.ToString()));
            }

            List<SelectListItem> floors = new List<SelectListItem>()   
            {
                new SelectListItem("Этаж", "0")
            };
            List<SelectListItem> types = new List<SelectListItem>()
            {
                new SelectListItem("Тип помещения", "0")
            };

            List<int> uniqueFloors = new List<int>();
            List<AudienceType> uniqueTypes = new();
            for (int i = 0; i < result.Count; i++)
            {
                //buildings.Add(new SelectListItem(result[i][0].Building.Title, result[i][0].Building.Id.ToString()));
                uniqueFloors.AddRange(result[i].Select(x => x.Floor));
                uniqueTypes.AddRange(result[i].Select(X => X.Type));
            }
            uniqueFloors = uniqueFloors.Distinct().ToList();
            uniqueTypes = uniqueTypes.Distinct().ToList();


            for (int i = 0; i < uniqueFloors.Count; i++)
            {
                floors.Add(new SelectListItem(uniqueFloors[i].ToString(), uniqueFloors[i].ToString()));
            }
            for (int i = 0; i < uniqueTypes.Count; i++)
            {
                types.Add(new SelectListItem(uniqueTypes[i].GetDescription(), ((int)uniqueFloors[i]).ToString()));
            }

            var b = buildings.FirstOrDefault(x => x.Value == buildingId.ToString());
            if (b != null)
            {
                b.Selected = true;
            }
            var f = floors.FirstOrDefault(x => x.Value == floor.ToString());
            if (f != null)
            {
                f.Selected = true;
            }
            var t = types.FirstOrDefault(x => x.Value == type.ToString());
            if (t != null)
            {
                t.Selected = true;
            }

            return new AllAudiencesViewModel()
            {
                ListOfListsOfAudiences = result,
                BuildingOptions = buildings,
                FloorOptions = floors,
                AudienceTypeOptions = types,
                SelectedBuildingId = buildingId,
                SelectedFloor = floor,
                SelectedType = type
            };
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

            List<Building> buildings = _corpusManagementService.GetAllBuildings();
            List<SelectListItem> buildingsOptions = new();
            for (int i = 0; i < buildings.Count; i++)
            {
                buildingsOptions.Add(new SelectListItem(buildings[i].Title, buildings[i].Id.ToString()));
            }
            List<SelectListItem> typeOptions = new List<SelectListItem>()
            {
                new SelectListItem(AudienceType.ClassRoom.GetDescription(), ((int)AudienceType.ClassRoom).ToString()),
                new SelectListItem(AudienceType.Audience.GetDescription(), ((int)AudienceType.Audience).ToString()),
                new SelectListItem(AudienceType.AssemblyHall.GetDescription(), ((int)AudienceType.AssemblyHall).ToString())
            };

            return View("Audience",
                new AudienceViewModel()
                {
                    Building = audience.Building,
                    Floor = audience.Floor,
                    Number = audience.Number,
                    SeatPlaces = audience.SeatPlaces,
                    TablesCount = audience.TablesCount,
                    WorkComputersCount = audience.WorkComputersCount,
                    HasProjector = audience.HasProjector,
                    HasAudio = audience.HasAudio,
                    Title = audience.Title,
                    Type = audience.Type,
                    BuildingId = audience.Building.Id,
                    Description = audience.Description,
                    IsBlockedByAdmin = audience.IsBlockedByAdmin,
                    BuildingOptions = buildingsOptions,
                    TypeOptions = typeOptions
                });
        }

        public async Task<IActionResult> GetBuilding(int id)
        {
            var building = await _corpusManagementService.GetBuildingAsync(id);
            return View("Audience", new BuildingViewModel() 
            { 
                Title = building.Title, 
                Address = building.Address, 
                CodeLetter = building.CodeLetter,
            });
        }

        public async Task<IActionResult> AddAudience(AudienceViewModel model)
        {
            if (!ModelState.IsValid)
            {

            }
            var building = await _buildingsRepository.GetAsync(model.BuildingId);
            await _corpusManagementService.PushAudienceAsync(new Audience() 
            {
                Building = building, 
                Floor = model.Floor, 
                Type = model.Type == 0 ? AudienceType.ClassRoom : model.Type,
                Number = model.Number,
                Title = model.Title,
                Description = model.Description,
                HasAudio = model.HasAudio,
                HasProjector = model.HasProjector,
                TablesCount = model.TablesCount,
                SeatPlaces = model.SeatPlaces,
                WorkComputersCount = model.WorkComputersCount,
                IsBlockedByAdmin = model.IsBlockedByAdmin
            });
            return RedirectToAction("AdminPanel", "User");
        }

        public async Task<IActionResult> AddBuilding(BuildingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                
            }

            //сделать букву маленькой
            model.CodeLetter = model.CodeLetter.ToString().ToLower().ToCharArray()[0];
            await _corpusManagementService.PushBuildingAsync(new Building() 
            { 
                Title = model.Title, 
                Address = model.Address,
                CodeLetter = model.CodeLetter
            });
            List<Building> buildings = _corpusManagementService.GetAllBuildings();
            List<SelectListItem> buildingsOptions = new();
            for (int i = 0; i < buildings.Count; i++)
            {
                buildingsOptions.Add(new SelectListItem(buildings[i].Title, buildings[i].Id.ToString()));
            }
            return RedirectToAction("AdminPanel", "User", new AdminPanelViewModel() { Buildings = buildings, BuildingsOptions = buildingsOptions});
        }
    }
}