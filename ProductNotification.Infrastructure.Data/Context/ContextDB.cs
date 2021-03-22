using Microsoft.EntityFrameworkCore;
using ProductNotification.Domain.Entities;

namespace ProductNotification.Infrastructure.Data.Context
{
    public class ContextDB : DbContext
    {
        public ContextDB(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                if (!Database.IsInMemory())
                {
                    entity.HasNoKey();
                    entity.Ignore(e => e.Id);
                }
                else
                {
                    entity.HasKey(e => e.Codigo);
                }
            });

            modelBuilder.Entity<User>(entity =>
            {
                if (!Database.IsInMemory())
                {
                    entity.HasNoKey();
                    entity.Ignore(e => e.Id);
                }
                else
                {
                    entity.HasKey(e => e.Codigo);
                }
            });
        }
    }
}