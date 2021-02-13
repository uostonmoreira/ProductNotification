using ProductNotification.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductNotification.Domain.Interfaces.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : Base
    {
        public Task<int> InsertAsync(TEntity obj);
        public Task<int> DeleteAsync(TEntity obj);
        public Task<IEnumerable<TEntity>> GetAsync();
        public Task<TEntity> GetByIdAsync(int id);
        public Task<int> UpdateAsync(TEntity obj);
    }
}
