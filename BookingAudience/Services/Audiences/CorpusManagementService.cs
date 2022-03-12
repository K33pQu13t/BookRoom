using BookingAudience.DAL.Repositories;
using BookingAudience.DTO.Corpus;
using BookingAudience.Models;
using BookingAudience.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.Services.Audiences
{
    public class CorpusManagementService
    {
        private readonly IGenericRepository<Audience> _audiencesRepository;
        private readonly IGenericRepository<Building> _buildingsRepository;

        public CorpusManagementService(IGenericRepository<Audience> audiencesRepository, IGenericRepository<Building> buildingsRepository)
        {
            _audiencesRepository = audiencesRepository;
            _buildingsRepository = buildingsRepository;
            //Building b = new Building()
            //{
            //    Title = "Л корпус",
            //    CodeLetter = 'Л',
            //    Address = "somewhere"
            //};
            //PushBuildingAsync(b);
        }

        public async Task PushBuildingAsync(Building building)
        {
            await _buildingsRepository.CreateAsync(building);
        }

        public async Task<Building> GetBuildingAsync(int id)
        {
            return await _buildingsRepository.GetAsync(id);
        }

        public async Task PushAudienceAsync(Audience audience)
        {
            if (!string.IsNullOrEmpty(audience.Title))
                audience.Number = -1;
            await _audiencesRepository.CreateAsync(audience);
        }

        public async Task<Audience> GetAudienceAsync(int id)
        {
            return await _audiencesRepository.GetAsync(id);
        }

        public List<Audience> GetAllAudiences()
        {
            return _audiencesRepository.Get().ToList();
        }

        /// <summary>
        /// получить список списков аудиторий. 
        /// В каждом списке аудиторий аудитории только из одного билдинга. 
        /// Аудитории отсортированы по этажам и номерам кабинета
        /// </summary>
        /// <returns></returns>
        public List<List<Audience>> GetAllAudiencesSortedByBuildingAndNumber(int buildingId = 0, int floor = 0, int type = 0)
        {
            List<Audience> audiences = GetAllAudiences().ToList();
            if (audiences == null || audiences.Count == 0)
                return null;

            //сортировки
            if (buildingId != 0)
                audiences = audiences.Where(a => a.Building.Id == buildingId).ToList();
            if (floor != 0)
                audiences = audiences.Where(a => a.Floor == floor).ToList();
            if (type != 0)
                audiences = audiences.Where(a => (int)(a.Type) == type).ToList();

            audiences = audiences.OrderBy(a => a.Building.Title).ToList();
            List<Building> buildings = audiences.Select(o => o.Building).Distinct().ToList();

            List<List<Audience>> result = new List<List<Audience>>();
            for (int i = 0; i < buildings.Count; i++)
            {
                result.Add(audiences.Where(
                    a => a.Building.Title == buildings[i].Title)
                    .OrderBy(a => a.Floor).OrderBy(a => a.Number).ToList());
            }
            return result;
        }

        public List<Building> GetAllBuildings()
        {
            return _buildingsRepository.Get().ToList();
        }

        public void EditAudience(AudienceDTO audienceDTO)
        {
            Audience audience = new Audience()
            {
                Building = audienceDTO.Building,
                Description = audienceDTO.Description,
                Floor = audienceDTO.Floor,
                HasAudio = audienceDTO.HasAudio,
                HasProjector = audienceDTO.HasProjector,
                IsBlockedByAdmin = audienceDTO.IsBlockedByAdmin,
                Number = audienceDTO.Number,
                Title = audienceDTO.Title,
                SeatPlaces = audienceDTO.SeatPlaces,
                TablesCount = audienceDTO.TablesCount,
                WorkComputersCount = audienceDTO.WorkComputersCount
            };
            _audiencesRepository.Update(audience);
        }

        public void EditBuilding(BuildingDTO buildingDTO)
        {
            Building building = new Building()
            {
                Address = buildingDTO.Address,
                CodeLetter = buildingDTO.CodeLetter,
                Title = buildingDTO.Title
            };
            _buildingsRepository.Update(building);
        }
    }
}
