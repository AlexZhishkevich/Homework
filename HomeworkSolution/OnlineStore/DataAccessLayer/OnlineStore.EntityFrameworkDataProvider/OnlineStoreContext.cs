using Microsoft.EntityFrameworkCore;
using OnlineStore.EntityFrameworkDataProvider.Models;

namespace OnlineStore.EntityFrameworkDataProvider
{
    public class OnlineStoreContext : DbContext
    {
        public OnlineStoreContext(DbContextOptions<OnlineStoreContext> options) : base(options)
        {
        }

        // here we declare our entity sets
        public DbSet<CatalogEntity> Catalogs { get; set; }
        public DbSet<GoodEntity> Goods { get; set; }
        public DbSet<CatalogGood> CatalogGoods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // here we declare relation between our entity and table in DataBase
            modelBuilder.Entity<CatalogEntity>().ToTable("Catalog");
            modelBuilder.Entity<GoodEntity>().ToTable("Good");
            modelBuilder.Entity<CatalogGood>().ToTable("CatalogGood");

            // for "CatalogGood" entity and table we set composite primary key
            modelBuilder.Entity<CatalogGood>()
                .HasKey(c => new { c.CatalogId, c.GoodId });
        }
    }
}
