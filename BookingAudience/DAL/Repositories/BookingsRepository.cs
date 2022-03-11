using BookingAudience.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.DAL.Repositories
{
    public class BookingsRepository : IGenericRepository<Booking>
    {
        private readonly ApplicationDBContext context;

        public BookingsRepository(ApplicationDBContext context)
        {
            this.context = context;
        }

        public void Create(Booking item)
        {
            context.Bookings.Add(item);
            context.SaveChanges();
        }

        public async Task<Booking> CreateAsync(Booking item)
        {
            var result = await context.Bookings.AddAsync(item);
            context.SaveChanges();
            return result.Entity;
        }

        public async Task<Booking> DeleteAsync(int id)
        {
            Booking userToDelete = await GetAsync(id);

            if (userToDelete != null)
            {
                context.Bookings.Remove(userToDelete);
                context.SaveChanges();
            }
            return userToDelete;
        }

        public void DeleteAll()
        {
            context.Bookings.RemoveRange(Get());
        }

        public IEnumerable<Booking> Get()
        {
            return context.Bookings;
        }

        public Booking Get(int id)
        {
            return context.Bookings.FirstOrDefault(u => u.Id == id);
        }

        public async Task<Booking> GetAsync(int id)
        {
            return await context.Bookings.FirstOrDefaultAsync(u => u.Id == id);
        }

        public void Update(Booking updatedItem)
        {
            context.Bookings.Update(updatedItem);
            context.SaveChanges();
        }
    }
}
