using eclipseworks.Domain.Contracts.Repositories.Project;
using eclipseworks.Infrastructure.EntitiesModels;

namespace eclipseworks.Infrastructure.Repositories.Project
{
    public class ProjectRepository : RepositoryBase<ProjectModel>, IProjectRepository<ProjectModel> { }
}
