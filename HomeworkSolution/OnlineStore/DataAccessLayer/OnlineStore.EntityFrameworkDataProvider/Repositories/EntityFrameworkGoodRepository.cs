using System;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Contracts.Interfaces;
using OnlineStore.Contracts.Models;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStore.EntityFrameworkDataProvider.Repositories
{
    public class EntityFrameworkGoodRepository : IGoodRepository
    {
        private readonly DbContextOptions<OnlineStoreContext> _databaseContextOptions;

        /// <summary>
        /// Create good repository with database context options
        /// </summary>
        /// <param name="databaseContextOptions">options for database connection</param>
        public EntityFrameworkGoodRepository(DbContextOptions<OnlineStoreContext> databaseContextOptions)
        {
            _databaseContextOptions = databaseContextOptions;
        }

        /// <summary>
        /// Get all goods from repository
        /// </summary>
        /// <returns>Goods business models IEnumerable</returns>
        public ICollection<Good> GetAll()
        {
            ICollection<Good> resultModels;

            using (var context = new OnlineStoreContext(_databaseContextOptions))
            {
                // get collection of "Catalog" entities from database with related CatalogGoods
                resultModels = context.Goods
                    .Select(e => e.ToGoodModel())
                    .ToList();
            }

            return resultModels;
        }

        /// <summary>
        /// Get good model by specified id
        /// </summary>
        /// <param name="id">id of good to be received</param>
        /// <returns>Good business model with specified id</returns>
        public Good GetById(object id)
        {
            if (!int.TryParse(id.ToString(), out var intId))
                return null;

            Good result;

            using (var context = new OnlineStoreContext(_databaseContextOptions))
            {
                result = context.Goods
                    .FirstOrDefault(good => good.Id == intId)
                    .ToGoodModel();
            }

            return result;
        }

        /// <summary>
        /// Get goods collection of specified catalog model
        /// </summary>
        /// <param name="catalog">Catalog business model</param>
        /// <returns>collection of "Good" business models or null</returns>
        public ICollection<Good> GetGoodsOfCatalog(Catalog catalog)
        {
            ICollection<Good> result = null;

            using (var context = new OnlineStoreContext(_databaseContextOptions))
            {
                // get collection of "Catalog" entities from database with related CatalogGoods
                var resultCatalogEntity = context.Catalogs
                    .Where(catalogEntity => catalogEntity.Id == catalog.Id) // get catalog entity with Id that equals with Id of business model
                    .Include(catalogEntity => catalogEntity.CatalogGoods) // add related instances of CatalogGoods table
                        .ThenInclude(catalogGood => catalogGood.Good)
                    .FirstOrDefault(); // add related instances of Goods table

                if (resultCatalogEntity != null)
                {
                    result = resultCatalogEntity.ToCatalogModel().Goods;
                }
            }

            return result;
        }

        /// <summary>
        /// Try to delete good from database
        /// </summary>
        /// <param name="entityToDelete">"Good" business model to be deleted</param>
        /// <returns>Deletion success</returns>
        public bool TryDelete(Good entityToDelete)
        {
            bool result = true;

            using (var context = new OnlineStoreContext(_databaseContextOptions))
            {
                try
                {
                    context.Goods.Remove(entityToDelete.ToGoodEntity());
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
        /// Try to delete good by specified Id
        /// </summary>
        /// <param name="entityId">id of "Good" model to be deleted</param>
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
                var goodToBeRemoved = context.Goods.FirstOrDefault(catalog => catalog.Id == intId);

                if (goodToBeRemoved != null)
                {
                    context.Goods.Remove(goodToBeRemoved);
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
        /// Try to add good to database
        /// </summary>
        /// <param name="good">"Good" business model to be added to database</param>
        /// <returns></returns>
        public bool TryInsert(Good good)
        {
            bool success = true;

            using (var context = new OnlineStoreContext(_databaseContextOptions))
            {
                var goodEntityAlreadyExists = context.Goods.Any(goodEntity => goodEntity.Id == good.Index);

                if (goodEntityAlreadyExists)
                {
                    success = false;
                }
                else
                {
                    context.Goods.Add(good.ToGoodEntity());
                    context.SaveChanges();
                }
            }

            return success;
        }

        /// <summary>
        /// Try to update "Good" model in database
        /// </summary>
        /// <param name="good">"Good" business model</param>
        /// <returns>update success</returns>
        public bool TryUpdate(Good good)
        {
            bool success = true;

            using (var context = new OnlineStoreContext(_databaseContextOptions))
            {
                var goodEntityToBeUpdated = context.Goods.FirstOrDefault(goodEntity => goodEntity.Id == good.Index);

                if (goodEntityToBeUpdated != null)
                {
                    context.Goods.Update(good.ToGoodEntity());
                    context.SaveChanges();
                }
                else
                {
                    success = false;
                }
            }

            return success;
        }
    }
}
