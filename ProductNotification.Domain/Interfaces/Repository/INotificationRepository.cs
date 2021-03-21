using ProductNotification.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductNotification.Domain.Interfaces.Repository
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetByFilterAsync(int productId);
    }
}