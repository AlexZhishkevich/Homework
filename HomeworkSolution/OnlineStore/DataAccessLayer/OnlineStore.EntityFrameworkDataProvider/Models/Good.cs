using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.EntityFrameworkDataProvider.Models
{
    public class GoodEntity
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public float Price { get; set; }

        // collection with relationships of this "Good" instance with "Catalog" entities 
        public ICollection<CatalogGood> CatalogGoods { get; set; }
    }
}
