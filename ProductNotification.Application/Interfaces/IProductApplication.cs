using ProductNotification.Domain.Entities;
using System.Threading.Tasks;

namespace ProductNotification.Application.Interfaces
{
    public interface IProductApplication
    {
        public Task<int> InsertAsync(Product product);
    }
}
