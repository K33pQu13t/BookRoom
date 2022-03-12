using BookingAudience.DAL.Repositories;
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
        public List<List<Audience>> GetAllAudiencesSortedByBuildingAndNumber()
        {
            var audiences = GetAllAudiences();
            if (audiences == null || audiences.Count == 0)
                return null;
            audiences = audiences.OrderBy(a => a.Building.Title).ToList();
            List<string> buildingTitles = audiences.Select(o => o.Building.Title).Distinct().ToList();

            List<List<Audience>> result = new List<List<Audience>>();
            for (int i = 0; i < buildingTitles.Count; i++)
            {
                result.Add(audiences.Where(a => a.Building.Title == buildingTitles[i]).OrderBy(a => a.Floor).OrderBy(a => a.Number).ToList());
            }
            return result;
        }

        public List<Building> GetAllBuildings()
        {
            return _buildingsRepository.Get().ToList();
        }
    }
}
