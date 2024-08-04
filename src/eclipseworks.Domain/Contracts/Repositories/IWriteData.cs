using System.Data;

namespace eclipseworks.Domain.Contracts.Repositories
{
    public interface IWriteData<TEntity> where TEntity : class
    {
        void SetTransaction(IDbTransaction dbTransaction);
        Task Insert(TEntity entity);
        Task Update(TEntity entity);
        Task<bool> Delete(TEntity entity);
    }
}
