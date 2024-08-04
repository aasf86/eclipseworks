using Dapper;
using eclipseworks.Domain.Contracts.Repositories.Taske;
using eclipseworks.Infrastructure.EntitiesModels;

namespace eclipseworks.Infrastructure.Repositories.Taske
{
    public class TaskeCommentRepository : RepositoryBase<TaskeCommentModel>, ITaskeCommentRepository<TaskeCommentModel> 
    {
        public override async Task<List<TaskeCommentModel?>> GetAll(dynamic filter)
        {
            var sql = $@"
                select *
                from {TaskeCommentModel.TableName} t
                where t.comment ilike '%'||@filter||'%'";

            return (await DbTransaction.Connection.QueryAsync<TaskeCommentModel?>(sql, new { filter })).ToList();
        }
    }
}
