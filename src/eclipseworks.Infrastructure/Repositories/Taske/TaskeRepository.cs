using Dapper;
using eclipseworks.Domain.Contracts.Repositories.Taske;
using eclipseworks.Infrastructure.EntitiesModels;
using static Dapper.SqlMapper;

namespace eclipseworks.Infrastructure.Repositories.Taske
{
    public class TaskeRepository : RepositoryBase<TaskeModel>, ITaskeRepository<TaskeModel> 
    {
        public override async Task<TaskeModel?> GetById(long id)
        {            
            var sql = @"
                select  
                    t.id,
                    t.guid,
                    t.inserted,
                    t.updated,
                    t.lasteventbyuser,
                    t.projectid,
                    t.title,
                    t.description,
                    t.expires,
	                lts.value status,
	                ltp.value priority,
                    t.userowner
                from 
	                taske t,
	                lv_taske_priority ltp,
	                lv_taske_status lts
                where
	                t.priorityid = ltp.id
                and t.statusid = lts.id
                and t.id = @id";

            var result = await DbTransaction.Connection.QuerySingleOrDefaultAsync<TaskeModel?>(sql, new { id });
            return result;
        }
        
        public override async Task<List<TaskeModel?>> GetAll(dynamic filter)
        {
            var sql = Helpers.StrSql.CreateSqlSelect<TaskeModel>(@" 
                title ilike '%'||@title||'%'
                order by title");

            return (await DbTransaction.Connection.QueryAsync<TaskeModel?>(sql, new { title = filter })).ToList();
        }
    }
}
