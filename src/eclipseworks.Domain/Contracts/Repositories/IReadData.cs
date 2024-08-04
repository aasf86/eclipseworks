using System.Data;

namespace eclipseworks.Domain.Contracts.Repositories
{
    public interface IReadData<TEntity> where TEntity : class
    {
        void SetTransaction(IDbTransaction dbTransaction);
        Task<TEntity?> GetById(long id);
        Task<List<TEntity?>> GetAll(dynamic filter);
    }
}
