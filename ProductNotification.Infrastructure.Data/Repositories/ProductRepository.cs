using Microsoft.EntityFrameworkCore;
using ProductNotification.Domain.Entities;
using ProductNotification.Domain.Interfaces.Repository;
using ProductNotification.Infrastructure.Data.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductNotification.Infrastructure.Data.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ContextDB db) : base(db)
        {
        }
        public async Task<IEnumerable<Product>> GetByPriceAsync(decimal price)
        {
            return await Db.Set<Product>().Where(p => p.Price == price).ToListAsync();
        }
    }
}