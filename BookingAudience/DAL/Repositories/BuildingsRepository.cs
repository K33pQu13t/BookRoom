using BookingAudience.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.DAL.Repositories
{
    public class BuildingsRepository : IGenericRepository<Building>
    {
        private readonly ApplicationDBContext context;

        public BuildingsRepository(ApplicationDBContext context)
        {
            this.context = context;
        }

        public void Create(Building item)
        {
            context.Buildings.Add(item);
            context.SaveChanges();
        }

        public async Task<Building> CreateAsync(Building item)
        {
            var result = await context.Buildings.AddAsync(item);
            context.SaveChanges();
            return result.Entity;
        }

        public async Task<Building> DeleteAsync(int id)
        {
            Building userToDelete = await GetAsync(id);

            if (userToDelete != null)
            {
                context.Buildings.Remove(userToDelete);
                context.SaveChanges();
            }
            return userToDelete;
        }

        public void DeleteAll()
        {
            context.Buildings.RemoveRange(Get());
        }

        public IEnumerable<Building> Get()
        {
            return context.Buildings;
        }

        public Building Get(int id)
        {
            return context.Buildings.FirstOrDefault(u => u.Id == id);
        }

        public async Task<Building> GetAsync(int id)
        {
            return await context.Buildings.FirstOrDefaultAsync(u => u.Id == id);
        }

        public void Update(Building updatedItem)
        {
            context.Buildings.Update(updatedItem);
            context.SaveChanges();
        }
    }
}
