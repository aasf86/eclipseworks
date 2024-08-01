using Dapper;
using Dapper.Contrib.Extensions;
using eclipseworks.Domain.Contracts.Repositories;
using eclipseworks.Domain.Entities;
using System.Data;

namespace eclipseworks.Infrastructure.Repositories
{
    //aasf86 retirar rascunho
    public abstract class WriteDataBase<TEntity> : IWriteData<TEntity> where TEntity : EntityBase
    {
        private IDbTransaction _dbTransaction;
        internal IDbTransaction DbTransaction => _dbTransaction;        
        /*
        private string? _sqlInsert;
        private string SqlInsert => _sqlInsert = _sqlInsert ?? Helpers.StrSql.CreateSqlInsert<TEntity>();

        private string? _sqlUdapte;
        private string SqlUdapte => _sqlUdapte = _sqlUdapte ?? Helpers.StrSql.CreateSqlUpdate<TEntity>();

        private string? _sqlDelete;
        private string SqlDelete => _sqlDelete = _sqlDelete ?? Helpers.StrSql.CreateSqlDelete<TEntity>();
        */
        public virtual void SetTransaction(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public virtual async Task<bool> Delete(long id)
        {
            var obj = Activator.CreateInstance(typeof(TEntity)) as TEntity;
            obj.Id = id;
            return (await DbTransaction.Connection.DeleteAsync(obj));
            //return (await DbTransaction.Connection.ExecuteAsync(SqlDelete, new { id })) > 0;
        }

        public virtual async Task Insert(TEntity entity)
        {
            await DbTransaction.Connection.InsertAsync(entity);
            //entity.Id = await DbTransaction.Connection.ExecuteScalarAsync<int>(SqlInsert, entity);
        }

        public virtual async Task Update(TEntity entity)
        {
            await DbTransaction.Connection.UpdateAsync(entity);
            //await DbTransaction.Connection.ExecuteAsync(SqlUdapte, entity);
        }
    }
}
