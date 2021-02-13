using ProductNotification.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductNotification.Domain.Interfaces.Repository
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        public Task<IEnumerable<Product>> GetByPriceAsync(decimal price);
    }
}
