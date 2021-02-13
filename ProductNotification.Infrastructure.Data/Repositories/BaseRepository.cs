using Microsoft.EntityFrameworkCore;
using ProductNotification.Domain.Entities;
using ProductNotification.Domain.Interfaces.Repository;
using ProductNotification.Infrastructure.Data.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductNotification.Infrastructure.Data.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : Base
    {
        protected ContextDB Db;

        public BaseRepository(ContextDB db)
        {
            Db = db;
        }

        public async Task<int> InsertAsync(TEntity obj)
        {
            await Db.Set<TEntity>().AddAsync(obj);
            return await Db.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(TEntity obj)
        {
            Db.Set<TEntity>().Remove(obj);
            return await Db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAsync()
        {
            return await Db.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await Db.Set<TEntity>().FindAsync(id);
        }

        public async Task<int> UpdateAsync(TEntity obj)
        {
            Db.Entry(obj).State = EntityState.Modified;
            return await Db.SaveChangesAsync();
        }
    }
}