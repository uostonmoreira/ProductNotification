using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProductNotification.Domain.Entities;
using ProductNotification.Domain.Interfaces.Repository;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProductNotification.Infrastructure.Data.MongoDB
{
    public class MongoRepository<TEntity> : IMongoRepository<TEntity> where TEntity : Base
    {
        private readonly IMongoCollection<TEntity> _mongoCollection;

        public MongoRepository(IMongoDB mongoDB)
        {
            string collectionName = Regex.Replace(typeof(TEntity).Name, "[^a-zA-Z]+", "").ToLower().Replace("entidade", "");
            this._mongoCollection = mongoDB.GetDatabase().GetCollection<TEntity>(collectionName, new MongoCollectionSettings { });
        }
        public async Task<DeleteResult> DeleteAsync(string id)
        {
            try
            {
                return await this._mongoCollection.DeleteOneAsync(entity => entity.Id.Equals(id));
            }
            catch (MongoClientException ex) { throw ex; }
            catch (MongoConnectionException ex) { throw ex; }
            catch (MongoInternalException ex) { throw ex; }
            catch (MongoServerException ex) { throw ex; }
            catch (MongoException ex) { throw ex; }
        }

        public IMongoQueryable<TEntity> GetAsync()
        {
            try
            {
                return this._mongoCollection.AsQueryable();
            }
            catch (MongoClientException ex) { throw ex; }
            catch (MongoConnectionException ex) { throw ex; }
            catch (MongoInternalException ex) { throw ex; }
            catch (MongoServerException ex) { throw ex; }
            catch (MongoException ex) { throw ex; }
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            try
            {
                var q = await this._mongoCollection.FindAsync(x => x.Id.Equals(id));
                var titulos = await q.ToListAsync();

                if (!titulos.Any())
                    return null;
                else
                    return titulos.First();
            }
            catch (MongoClientException ex) { throw ex; }
            catch (MongoConnectionException ex) { throw ex; }
            catch (MongoInternalException ex) { throw ex; }
            catch (MongoServerException ex) { throw ex; }
            catch (MongoException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
        }

        public async Task InsertAsync(TEntity entity)
        {
            try
            {
                await this._mongoCollection.InsertOneAsync(entity);
            }
            catch (MongoClientException ex) { throw ex; }
            catch (MongoConnectionException ex) { throw ex; }
            catch (MongoInternalException ex) { throw ex; }
            catch (MongoServerException ex) { throw ex; }
            catch (MongoException ex) { throw ex; }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            try
            {
                return await this._mongoCollection.FindOneAndReplaceAsync(upd => upd.Id.Equals(entity.Id), entity);
            }
            catch (MongoClientException ex) { throw ex; }
            catch (MongoConnectionException ex) { throw ex; }
            catch (MongoInternalException ex) { throw ex; }
            catch (MongoServerException ex) { throw ex; }
            catch (MongoException ex) { throw ex; }
        }
    }
}