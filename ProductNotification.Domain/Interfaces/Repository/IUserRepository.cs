using ProductNotification.Domain.Entities;
using System.Threading.Tasks;

namespace ProductNotification.Domain.Interfaces.Repository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        public Task<User> GetByUserPassAsync(string user, string pass);
    }
}