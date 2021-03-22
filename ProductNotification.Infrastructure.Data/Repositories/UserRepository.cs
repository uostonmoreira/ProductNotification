using Microsoft.EntityFrameworkCore;
using ProductNotification.Domain.Entities;
using ProductNotification.Domain.Interfaces.Repository;
using ProductNotification.Infrastructure.Data.Context;
using System.Linq;
using System.Threading.Tasks;

namespace ProductNotification.Infrastructure.Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ContextDB db) : base(db)
        {
        }

        public async Task<User> GetByUserPassAsync(string user, string pass)
        {
            return await Db.Set<User>().Where(u => u.UserName.Equals(user) && u.Password.Equals(pass)).FirstOrDefaultAsync();
        }
    }
}