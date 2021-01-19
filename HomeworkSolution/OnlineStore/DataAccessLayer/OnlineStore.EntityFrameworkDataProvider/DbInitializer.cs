using OnlineStore.EntityFrameworkDataProvider.Models;
using System.Linq;

namespace OnlineStore.EntityFrameworkDataProvider
{
    public static class DbInitializer
    {
        public static void InitializeWithTestData(OnlineStoreContext context)
        {
            context.Database.EnsureCreated();

            // Look for any catalogs.
            if (context.Catalogs.Any())
            {
                return;   // DB has been seeded
            }

            // Create array of catalogs
            var catalogs = new CatalogEntity[]
            {
                new CatalogEntity(){Name = "Household appliances"},
                new CatalogEntity(){Name = "Work and office"},
                new CatalogEntity(){Name = "Beauty and sport"},
                new CatalogEntity(){Name = "Electronics"},
                new CatalogEntity(){Name = "Computers and networks"},
            };

            // Add created "Catalog" instances to DataBase in loop
            foreach (var catalog in catalogs)
            {
                context.Catalogs.Add(catalog);
            }
            context.SaveChanges();

            // Create array of goods
            var goods = new GoodEntity[]
            {
                new GoodEntity(){Name = "Graphics card", Price = 340},
                new GoodEntity(){Name = "Processor", Price = (float)250.5},
                new GoodEntity(){Name = "SSD", Price = 120},
                new GoodEntity(){Name = "TV tuner", Price = 100},
                new GoodEntity(){Name = "Vacuum cleaner", Price = 190},
                new GoodEntity(){Name = "Coffee machine", Price = 200},
                new GoodEntity(){Name = "Conditioner", Price = 500},
                new GoodEntity(){Name = "Bicycle", Price = 420},
                new GoodEntity(){Name = "Exercise bike", Price = 700},
                new GoodEntity(){Name = "Fitness bracelet", Price = 110},
                new GoodEntity(){Name = "Laptop", Price = 1200},
                new GoodEntity(){Name = "Printer", Price = 390},
                new GoodEntity(){Name = "Flash drive", Price = 3},
            };

            // Add created "Good" instances to DataBase in loop
            foreach (var good in goods)
            {
                context.Goods.Add(good);
            }
            context.SaveChanges();

            var catalogGoods = new CatalogGood[]
            {
                new CatalogGood(){CatalogId = 2, GoodId = 1},
                new CatalogGood(){CatalogId = 2, GoodId = 2},
                new CatalogGood(){CatalogId = 4, GoodId = 2},
                new CatalogGood(){CatalogId = 5, GoodId = 2},
                new CatalogGood(){CatalogId = 2, GoodId = 3},
                new CatalogGood(){CatalogId = 4, GoodId = 3},
                new CatalogGood(){CatalogId = 5, GoodId = 3},
                new CatalogGood(){CatalogId = 1, GoodId = 4},
                new CatalogGood(){CatalogId = 4, GoodId = 4},
                new CatalogGood(){CatalogId = 1, GoodId = 5},
                new CatalogGood(){CatalogId = 1, GoodId = 6},
                new CatalogGood(){CatalogId = 2, GoodId = 6},
                new CatalogGood(){CatalogId = 1, GoodId = 7},
                new CatalogGood(){CatalogId = 2, GoodId = 7},
                new CatalogGood(){CatalogId = 3, GoodId = 8},
                new CatalogGood(){CatalogId = 3, GoodId = 9},
                new CatalogGood(){CatalogId = 3, GoodId = 10},
                new CatalogGood(){CatalogId = 4, GoodId = 10},
                new CatalogGood(){CatalogId = 2, GoodId = 11},
                new CatalogGood(){CatalogId = 5, GoodId = 11},
                new CatalogGood(){CatalogId = 2, GoodId = 12},
                new CatalogGood(){CatalogId = 2, GoodId = 13},
                new CatalogGood(){CatalogId = 4, GoodId = 13},
                new CatalogGood(){CatalogId = 5, GoodId = 13},
            };

            // Add created "CatalogGood" instances to DataBase in loop
            foreach (var catalogGood in catalogGoods)
            {
                context.CatalogGoods.Add(catalogGood);
            }
            context.SaveChanges();
        }
    }
}
