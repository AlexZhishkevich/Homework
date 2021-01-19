namespace OnlineStore.EntityFrameworkDataProvider.Models
{
    public class CatalogGood
    {
        /// <summary>
        /// Foreign key to entity "Catalog"
        /// </summary>
        public int CatalogId { get; set; }
        /// <summary>
        /// Foreign key to entity "Good"
        /// </summary>
        public int GoodId { get; set; }

        /// <summary>
        /// Related "Catalog" entity by foreign key
        /// </summary>
        public CatalogEntity Catalog { get; set; }
        /// <summary>
        /// Related "Good" entity by foreign key
        /// </summary>
        public GoodEntity Good { get; set; }
    }
}
