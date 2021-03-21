using MongoDB.Driver;
using ProductNotification.Domain.Interfaces.Repository;
using System;

namespace ProductNotification.Infrastructure.Data.MongoDB
{
    public class MongoDB : IMongoDB
    {
        private readonly IMongoDatabase _mongoDatabase;

        public MongoDB(string connectionString, string dataBase)
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.ConnectTimeout = new TimeSpan(0, 0, 0, 9000);
            settings.MaxConnectionIdleTime = new TimeSpan(0, 0, 0, 9000);
            settings.SocketTimeout = new TimeSpan(0, 0, 0, 0, 9000);
            settings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
            IMongoClient mongoClient = new MongoClient(settings);
            _mongoDatabase = mongoClient.GetDatabase(dataBase);
        }

        public IMongoDatabase GetDatabase()
        {
            return this._mongoDatabase;
        }
    }
}