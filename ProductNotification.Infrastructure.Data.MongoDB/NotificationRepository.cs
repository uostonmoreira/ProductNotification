using MongoDB.Driver;
using ProductNotification.Domain.Entities;
using ProductNotification.Domain.Interfaces.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductNotification.Infrastructure.Data.MongoDB
{
    public class NotificationRepository : MongoRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(IMongoDB mongoDB)
            : base(mongoDB)
        {}
        public async Task<IEnumerable<Notification>> GetByFilterAsync(int productId)
        {
            var filter = Builders<Notification>.Filter.Eq(x => x.ProductId, productId);
            var notificatios = await this.GetAsync(filter);

            return notificatios;
        }
    }
}