using System.Collections.Generic;

namespace OnlineStore.Contracts.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        bool TryInsert(TEntity entity);

        bool TryUpdate(TEntity entityToUpdate);

        bool TryDelete(TEntity entityToDelete);

        bool TryDeleteById(object entityId);

        ICollection<TEntity> GetAll();

        TEntity GetById(object id);
    }
}
