using System.Collections.Generic;

namespace OnlineStore.Contracts.Models
{
    public class Catalog
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Good> Goods { get; set; }
    }
}
