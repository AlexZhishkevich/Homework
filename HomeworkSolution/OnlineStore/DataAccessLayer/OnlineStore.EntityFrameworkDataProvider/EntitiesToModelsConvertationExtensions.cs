using System.Collections.Generic;
using System.Linq;
using OnlineStore.Contracts.Models;
using OnlineStore.EntityFrameworkDataProvider.Models;

namespace OnlineStore.EntityFrameworkDataProvider
{
    /// <summary>
    /// Class, that contains logic to transform Entity Framework models to business models
    /// </summary>
    public static class EntitiesToModelsConvertationExtensions
    {
        /// <summary>
        /// Convert current "Good" database entity to "Good" business model
        /// </summary>
        /// <param name="goodEntity">"Good" entity from database</param>
        /// <returns>"Good" business model</returns>
        internal static Good ToGoodModel(this GoodEntity goodEntity)
        {
            return new Good()
            {
                Index = goodEntity.Id,
                Name = goodEntity.Name,
                Price = goodEntity.Price
            };
        }

        /// <summary>
        /// Convert "Good" business model to "Good" database entity
        /// </summary>
        /// <param name="good">"Good" business model</param>
        /// <returns>"Good" entity model</returns>
        internal static GoodEntity ToGoodEntity(this Good good)
        {
            return new GoodEntity()
            {
                Id= good.Index,
                Name = good.Name,
                Price = good.Price
            };
        }

        /// <summary>
        /// Convert current "Catalog" database entity to "Catalog" business model with inner Goods collection
        /// </summary>
        /// <param name="catalogEntity">"Catalog" entity from database</param>
        /// <returns>"Catalog" business model with inner Goods collection</returns>
        internal static Catalog ToCatalogModel(this CatalogEntity catalogEntity)
        {
            return new Catalog()
            {
                Id = catalogEntity.Id,
                Name = catalogEntity.Name,
                Goods = catalogEntity.CatalogGoods
                    .Select(catalogGood => catalogGood.Good // select goods entities by related CatalogGood property
                        .ToGoodModel())  //and then convert each to "Good" business model
                    .ToList()
            };
        }

        /// <summary>
        /// Convert current "Catalog" business model to "Catalog" entity
        /// </summary>
        /// <param name="catalogBusinessModel">"Catalog" business model</param>
        /// <returns>"Catalog" entity</returns>
        internal static CatalogEntity ToCatalogEntity(this Catalog catalogBusinessModel)
        {
            return new CatalogEntity()
            {
                Id = catalogBusinessModel.Id,
                Name = catalogBusinessModel.Name
            };
        }
    }
}
