using eclipseworks.Domain.Entities;
using System.Data;

namespace eclipseworks.Domain.Contracts.Repositories
{
    public interface IRepository<TEntity>
        : IWriteData<TEntity>, IReadData<TEntity>
        where TEntity : class
    {
        void SetTransaction(IDbTransaction dbTransaction);
    }
}
