using Dapper;
using eclipseworks.Domain.Contracts.Repositories.Project;
using eclipseworks.Infrastructure.EntitiesModels;
using static Dapper.SqlMapper;

namespace eclipseworks.Infrastructure.Repositories.Project
{
    public class ProjectRepository : RepositoryBase<ProjectModel>, IProjectRepository<ProjectModel> 
    {
        public override async Task<List<ProjectModel?>> GetAll(dynamic filter)
        {

            var sql = Helpers.StrSql.CreateSqlSelect<ProjectModel>(@" 
                name ilike '%'||@name||'%'
                order by name");

            return (await DbTransaction.Connection.QueryAsync<ProjectModel?>(sql, new { name = filter })).ToList();
        }
    }
}
