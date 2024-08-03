using Entity = eclipseworks.Domain.Entities;

namespace eclipseworks.Domain.Contracts.Repositories.Taske
{
    public interface ITaskeRepository<TTask> : IRepository<TTask> where TTask : Entity.Taske { }
}
