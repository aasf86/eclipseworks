using Dapper;
using eclipseworks.Domain.Contracts.Repositories.Taske;
using eclipseworks.Infrastructure.EntitiesModels;
using static Dapper.SqlMapper;

namespace eclipseworks.Infrastructure.Repositories.Taske
{
    public class TaskeRepository : RepositoryBase<TaskeModel>, ITaskeRepository<TaskeModel> 
    {
        public override async Task<List<TaskeModel?>> GetAll(dynamic filter)
        {

            var sql = Helpers.StrSql.CreateSqlSelect<TaskeModel>(@" 
                title ilike '%'||@title||'%'
                order by title");

            return (await DbTransaction.Connection.QueryAsync<TaskeModel?>(sql, new { title = filter })).ToList();
        }
    }
}
