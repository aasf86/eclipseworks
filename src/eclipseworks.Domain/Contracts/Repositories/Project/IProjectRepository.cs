using Entity = eclipseworks.Domain.Entities;

namespace eclipseworks.Domain.Contracts.Repositories.Project
{
    public interface IProjectRepository<TProject> : IRepository<TProject> where TProject : Entity.Project { }
}
