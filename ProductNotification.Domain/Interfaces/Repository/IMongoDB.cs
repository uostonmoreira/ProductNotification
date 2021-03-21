using MongoDB.Driver;

namespace ProductNotification.Domain.Interfaces.Repository
{
    public interface IMongoDB
    {
        public IMongoDatabase GetDatabase();
    }
}
