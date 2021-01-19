using System.Collections.Generic;
using OnlineStore.Contracts.Models;

namespace OnlineStore.Contracts.Interfaces
{
    public interface IGoodRepository : IRepository<Good>
    {
        public ICollection<Good> GetGoodsOfCatalog(Catalog catalog);
    }
}
