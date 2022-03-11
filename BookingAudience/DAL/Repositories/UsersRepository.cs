using BookingAudience.Models.Users;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.DAL.Repositories
{
    public class UsersRepository : IGenericRepository<AppUser>
    {
        private readonly ApplicationDBContext context;

        public UsersRepository(ApplicationDBContext context)
        {
            this.context = context;
        }

        public void Create(AppUser item)
        {
            context.Users.Add(item);
            context.SaveChanges();
        }

        public async Task<AppUser> CreateAsync(AppUser item)
        {
            var result = await context.Users.AddAsync(item);
            context.SaveChanges();
            return result.Entity;
        }

        public async Task<AppUser> DeleteAsync(int id)
        {
            AppUser userToDelete = await GetAsync(id);

            if (userToDelete != null)
            {
                context.Users.Remove(userToDelete);
                context.SaveChanges();
            }
            return userToDelete;
        }

        public void DeleteAll()
        {
            context.Users.RemoveRange(Get());
        }

        public IEnumerable<AppUser> Get()
        {
            return context.Users;
        }

        public AppUser Get(int id)
        {
            return context.Users.FirstOrDefault(u => u.Id == id);
        }

        public async Task<AppUser> GetAsync(int id)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public void Update(AppUser updatedItem)
        {
            context.Users.Update(updatedItem);
            context.SaveChanges();
        }
    }
}
