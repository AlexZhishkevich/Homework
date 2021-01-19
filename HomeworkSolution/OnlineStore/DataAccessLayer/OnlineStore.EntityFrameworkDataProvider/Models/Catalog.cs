using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.EntityFrameworkDataProvider.Models
{
    public class CatalogEntity
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        // collection with relationships of this "Catalog" instance with "Goods" entities 
        public ICollection<CatalogGood> CatalogGoods { get; set; }
    }
}
