using BookingAudience.DAL;
using BookingAudience.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.DAL.Repositories
{
    public class AudiencesRepository : IGenericRepository<Audience>
    {
        private readonly ApplicationDBContext context;

        public AudiencesRepository(ApplicationDBContext context)
        {
            this.context = context;
        }

        public void Create(Audience item)
        {
            context.Audiences.Add(item);
            context.SaveChanges();
        }

        public async Task<Audience> CreateAsync(Audience item)
        {
            var result = await context.Audiences.AddAsync(item);
            context.SaveChanges();
            return result.Entity;
        }

        public async Task<Audience> DeleteAsync(int id)
        {
            Audience userToDelete = await GetAsync(id);

            if (userToDelete != null)
            {
                context.Audiences.Remove(userToDelete);
                context.SaveChanges();
            }
            return userToDelete;
        }

        public void DeleteAll()
        {
            context.Audiences.RemoveRange(Get());
        }

        public IEnumerable<Audience> Get()
        {
            return context.Audiences;
        }

        public Audience Get(int id)
        {
            return context.Audiences.FirstOrDefault(u => u.Id == id);
        }

        public async Task<Audience> GetAsync(int id)
        {
            return await context.Audiences.FirstOrDefaultAsync(u => u.Id == id);
        }

        public void Update(Audience updatedItem)
        {
            context.Audiences.Update(updatedItem);
            context.SaveChanges();
        }
    }
}
