using Dapper;
using Dapper.Contrib.Extensions;
using eclipseworks.Domain.Contracts.Repositories;
using eclipseworks.Domain.Entities;
using System.Data;
using System.Reflection;
using System.Text;

namespace eclipseworks.Infrastructure.Repositories
{
    //aasf86 retirar rascunho
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        #region Properties/Fields

        private IDbTransaction _dbTransaction;
        internal IDbTransaction DbTransaction => _dbTransaction;

        /*
        private string? _sqlInsert;
        private string SqlInsert => _sqlInsert = _sqlInsert ?? Helpers.StrSql.CreateSqlInsert<TEntity>();

        private string? _sqlUdapte;
        private string SqlUdapte => _sqlUdapte = _sqlUdapte ?? Helpers.StrSql.CreateSqlUpdate<TEntity>();

        private string? _sqlDelete;
        private string SqlDelete => _sqlDelete = _sqlDelete?? Helpers.StrSql.CreateSqlDelete<TEntity>();

        private string? _sqlSelect;
        private string SqlSelect => _sqlSelect = _sqlSelect ?? Helpers.StrSql.CreateSqlSelect<TEntity>();
        */

        #endregion

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

        public virtual async Task<List<TEntity?>> GetAll(dynamic filter)
        {
            return default;            
        }

        public virtual async Task<TEntity?> GetById(long id)
        {
            return await DbTransaction.Connection.GetAsync<TEntity?>(id);
            //return await DbTransaction.Connection.QuerySingleOrDefaultAsync<TEntity?>(SqlSelect, new { id });
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

    public class SqlAdapter : ISqlAdapter
    {
        public void AppendColumnName(StringBuilder sb, string columnName)
        {
            throw new NotImplementedException();
        }

        public void AppendColumnNameEqualsValue(StringBuilder sb, string columnName)
        {
            throw new NotImplementedException();
        }

        public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, string tableName, string columnList, string parameterList, IEnumerable<PropertyInfo> keyProperties, object entityToInsert)
        {
            throw new NotImplementedException();
        }
    }
}
