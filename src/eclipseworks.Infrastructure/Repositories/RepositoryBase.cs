using Dapper;
using eclipseworks.Domain.Contracts.Repositories;
using eclipseworks.Domain.Entities;
using System.Data;

namespace eclipseworks.Infrastructure.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        #region Properties/Fields

        private IDbTransaction _dbTransaction;
        internal IDbTransaction DbTransaction => _dbTransaction;

        
        private string? _sqlInsert;
        private string SqlInsert => _sqlInsert = _sqlInsert ?? Helpers.StrSql.CreateSqlInsert<TEntity>();

        private string? _sqlSelect;
        private string SqlSelect => _sqlSelect = _sqlSelect ?? Helpers.StrSql.CreateSqlSelect<TEntity>();        

        private string? _sqlUdapte;
        private string SqlUdapte => _sqlUdapte = _sqlUdapte ?? Helpers.StrSql.CreateSqlUpdate<TEntity>();

        private string? _sqlDelete;
        private string SqlDelete => _sqlDelete = _sqlDelete ?? Helpers.StrSql.CreateSqlDelete<TEntity>();

        #endregion

        public virtual void SetTransaction(IDbTransaction dbTransaction) 
        {
            _dbTransaction = dbTransaction;
        }

        public virtual async Task<bool> Delete(TEntity entity)
        {
            return (await DbTransaction.Connection.ExecuteAsync(SqlDelete, entity)) > 0;
        }

        public virtual async Task<List<TEntity?>> GetAll(dynamic filter)
        {
            return default;            
        }

        public virtual async Task<TEntity?> GetById(long id)
        {
            return await DbTransaction.Connection.QuerySingleOrDefaultAsync<TEntity?>(SqlSelect, new { id });
        }

        public virtual async Task Insert(TEntity entity)
        {
            var method = typeof(TEntity).GetProperty("Id");
            var id = await DbTransaction.Connection.ExecuteScalarAsync<long>(SqlInsert, entity);
            method.SetValue(entity, id);
        }

        public virtual async Task Update(TEntity entity)
        {         
            await DbTransaction.Connection.ExecuteAsync(SqlUdapte, entity);
        }
    }
}
