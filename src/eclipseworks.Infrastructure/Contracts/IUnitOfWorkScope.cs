namespace eclipseworks.Infrastructure.Contracts
{
    public interface IUnitOfWorkScope
    {
        Task UnitOfWorkExecute(Func<Task> execute);
    }
}
