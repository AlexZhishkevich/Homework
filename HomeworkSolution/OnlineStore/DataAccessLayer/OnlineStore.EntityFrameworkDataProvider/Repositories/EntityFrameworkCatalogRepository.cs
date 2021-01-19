using OnlineStore.Contracts.Interfaces;
using OnlineStore.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OnlineStore.EntityFrameworkDataProvider.Models;

namespace OnlineStore.EntityFrameworkDataProvider.Repositories
{
    public class EntityFrameworkCatalogRepository : ICatalogRepository
    {
        private readonly DbContextOptions<OnlineStoreContext> _databaseContextOptions;

        /// <summary>
        /// Create catalog repository with database context options
        /// </summary>
        /// <param name="databaseContextOptions">options for database connection</param>
        public EntityFrameworkCatalogRepository(DbContextOptions<OnlineStoreContext> databaseContextOptions)
        {
            _databaseContextOptions = databaseContextOptions;
        }

        /// <summary>
        /// Get all Catalogs from database
        /// </summary>
        /// <returns>Collection of "Catalog" business models</returns>
        public ICollection<Catalog> GetAll()
        {
            ICollection<Catalog> result;

            using (var context = new OnlineStoreContext(_databaseContextOptions))
            {
                // get collection of "Catalog" entities from database with related CatalogGoods
                var resultEntities = context.Catalogs
                    .Include(catalog => catalog.CatalogGoods) // add related instances of CatalogGoods table
                        .ThenInclude(catalogGood => catalogGood.Good).ToList(); // add related instances of Goods table

                // transform to collection of "Catalog" business models with inner "Good" collection
                result = resultEntities.Select(e => e.ToCatalogModel()).ToList();
            }

            return result;
        }

        /// <summary>
        /// Get Catalog model by id
        /// </summary>
        /// <param name="id">catalog id property</param>
        /// <returns></returns>
        public Catalog GetById(object id)
        {
            if (!int.TryParse(id.ToString(), out var intIndex))
                return null;

            Catalog result;

            using (var context = new OnlineStoreContext(_databaseContextOptions))
            {
                result = context.Catalogs
                    .Include(catalog => catalog.CatalogGoods)
                        .ThenInclude(catalogGood => catalogGood.Good)
                    .FirstOrDefault(catalog => catalog.Id == intIndex)
                    .ToCatalogModel();
            }

            return result;
        }

        /// <summary>
        /// Get all catalogs with good of specified Id
        /// </summary>
        /// <param name="goodId">"good" id to search by</param>
        /// <returns></returns>
        public ICollection<Catalog> GetCatalogsByGoodId(int goodId)
        {
            ICollection<Catalog> result;

            using (var context = new OnlineStoreContext(_databaseContextOptions))
            {
                result = context.Catalogs
                    .Include(catalog => catalog.CatalogGoods) // add related collection of CatalogGoods
                    .Where(catalog => catalog.CatalogGoods.Any(catalogGood => catalogGood.GoodId == goodId)) // select only catalogs that contains at least one CatalogGood with GoodId equals goodId 
                    .Select(catalog => catalog.ToCatalogModel()) // convert each found Catalog entities to Catalog business models 
                    .ToList(); // convert result IEnumerable to ICollection
            }

            return result;
        }

        /// <summary>
        /// Try to delete Catalog from database
        /// </summary>
        /// <param name="entityToDelete">"Catalog" business model to be deleted</param>
        /// <returns>Deletion success</returns>
        public bool TryDelete(Catalog entityToDelete)
        {
            bool result = true;

            using (var context = new OnlineStoreContext(_databaseContextOptions))
            {
                try
                {
                    context.Catalogs.Remove(entityToDelete.ToCatalogEntity());
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Try to delete Catalog by specified Id
        /// </summary>
        /// <param name="entityId">id of Catalog model to be deleted</param>
        /// <returns>deletion success</returns>
        public bool TryDeleteById(object entityId)
        {
            bool result = true;

            if (!int.TryParse(entityId.ToString(), out var intId))
            {
                return false;
            }

            using (var context = new OnlineStoreContext(_databaseContextOptions))
            {
                var catalogToBeRemoved = context.Catalogs.FirstOrDefault(catalog => catalog.Id == intId);

                if (catalogToBeRemoved != null)
                {
                    context.Catalogs.Remove(catalogToBeRemoved);
                    context.SaveChanges();
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Insert Catalog with inner goods collection
        /// </summary>
        /// <param name="catalog">"Catalog" business model</param>
        /// <returns>operation success</returns>
        public bool TryInsert(Catalog catalog)
        {
            using (var context = new OnlineStoreContext(_databaseContextOptions))
            {
                if (context.Catalogs.Any(entity => entity.Id == catalog.Id)) // if catalog entity already exists
                    return false;

                var catalogEntity = catalog.ToCatalogEntity();
                context.Add(catalogEntity); // we added catalog entity to database and now we have id that was determined by database
                context.SaveChanges();

                if (catalog.Goods != null)
                {
                    foreach (var good in catalog.Goods) // go through goods collection
                    {
                        if (!context.Goods.Any(goodEntity => goodEntity.Id == good.Index)) // if good with id does not exist, than create and add to database
                        {
                            var goodEntity = good.ToGoodEntity();
                            context.Goods.Add(goodEntity);
                            context.SaveChanges(); // we added good entity to database and now we have id that was determined by database

                            context.CatalogGoods.Add(new CatalogGood()
                            {
                                CatalogId = catalogEntity.Id,
                                GoodId = goodEntity.Id
                            });
                        }
                    }
                }

                context.SaveChanges();
            }

            return true;
        }

        /// <summary>
        /// Try to update "Catalog" model in database
        /// </summary>
        /// <param name="catalog">"Catalog" model</param>
        /// <returns>operation success</returns>
        public bool TryUpdate(Catalog catalog)
        {
            using (var context = new OnlineStoreContext(_databaseContextOptions))
            {
                var catalogEntityAlreadyExists = context.Catalogs.Any(catalogEntity => catalogEntity.Id == catalog.Id);

                if (!catalogEntityAlreadyExists) // if catalog entity already exists then return false
                    return false;

                if (catalog.Goods != null)
                {
                    foreach (var good in catalog.Goods) // go through goods collection
                    {
                        if (!context.Goods.Any(goodEntity => goodEntity.Id == good.Index)) // if good with id does not exist, than create and add to database
                        {
                            var goodEntity = good.ToGoodEntity();
                            context.Goods.Add(goodEntity);
                            context.SaveChanges(); // we added good entity to database and now we have id that was determined by database

                            if (!context.CatalogGoods.Any(catalogGood => catalogGood.GoodId == catalog.Id))
                            {
                                context.CatalogGoods.Add(new CatalogGood()
                                {
                                    CatalogId = catalog.Id,
                                    GoodId = goodEntity.Id
                                });
                            }
                        }
                    }
                }

                context.Update(catalog.ToCatalogEntity());
                context.SaveChanges();
            }

            return true;
        }
    }
}
