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

        public List<Building> GetAllBuildings()
        {
            return _buildingsRepository.Get().ToList();
        }
    }
}
