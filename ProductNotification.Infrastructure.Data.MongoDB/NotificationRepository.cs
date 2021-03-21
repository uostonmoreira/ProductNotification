using Faturamento.Agrupador.Dados.Utils;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProductNotification.Domain.Entities;
using ProductNotification.Domain.Interfaces.Repository;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
            
            var notificatios = await (
                      await this.GetAsync()
                          .Where(n => n.ProductId == productId)
                          .ToCursorAsync()
              ).FetchAllAsync();

            return notificatios;
        }
    }
}
