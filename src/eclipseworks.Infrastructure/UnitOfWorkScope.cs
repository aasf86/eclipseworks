﻿using eclipseworks.Infrastructure.Contracts;
using System.Data;

namespace eclipseworks.Infrastructure
{
    public abstract class UnitOfWorkScope : IUnitOfWorkScope
    {
        private readonly IDbConnection _dbConnection;
        private IDbConnection DbConnection => _dbConnection;

        private IDbTransaction _dbTransaction;
        private IDbTransaction DbTransaction => _dbTransaction;

        private readonly List<Action<IDbTransaction>> _transactionAssigner = new List<Action<IDbTransaction>>();
        public List<Action<IDbTransaction>> TransactionAssigner => _transactionAssigner;

        protected UnitOfWorkScope(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public virtual async Task UnitOfWorkExecute(Func<Task> execute)
        {            
            if (DbTransaction is not null)
            {
                TransactionAssigner.ForEach(x => x(DbTransaction));
                await execute();
                return;
            }            

            if (DbConnection.State == ConnectionState.Closed) DbConnection.Open();

            using (DbConnection)
            using (_dbTransaction = DbConnection.BeginTransaction())
            {
                try
                {
                    TransactionAssigner.ForEach(x => x(DbTransaction));
                    await execute();
                    DbTransaction.Commit();
                }
                catch
                {
                    DbTransaction.Rollback();
                    throw;
                }                
            }
        }
    }
}
