using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;

namespace ProductNotification.Domain.Interfaces.Repository
{
    public interface IMongoRepository<TEntity>
    {
        IMongoQueryable<TEntity> GetAsync();
        Task<TEntity> GetByIdAsync(string id);
        Task InsertAsync(TEntity entidade);
        Task<TEntity> UpdateAsync(TEntity entidade);
        Task<DeleteResult> DeleteAsync(string id);
    }
}