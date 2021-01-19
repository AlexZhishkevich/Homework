using System.Collections.Generic;
using OnlineStore.Contracts.Models;

namespace OnlineStore.Contracts.Interfaces
{
    public interface ICatalogRepository : IRepository<Catalog>
    {
        public ICollection<Catalog> GetCatalogsByGoodId(int goodId);
    }
}
