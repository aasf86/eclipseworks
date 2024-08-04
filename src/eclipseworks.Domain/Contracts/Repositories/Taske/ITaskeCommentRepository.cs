using Entity = eclipseworks.Domain.Entities;

namespace eclipseworks.Domain.Contracts.Repositories.Taske
{
    public interface ITaskeCommentRepository<TProject> : IRepository<TProject> where TProject : Entity.TaskeComment { }
}
