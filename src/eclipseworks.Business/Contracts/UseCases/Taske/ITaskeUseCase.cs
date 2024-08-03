using eclipseworks.Business.Dtos;
using eclipseworks.Business.Dtos.Taske;

namespace eclipseworks.Business.Contracts.UseCases.Taske
{
    public interface ITaskeUseCase : IValidators
    {        
        Task<ResponseBase<TaskeInsert>> Insert(RequestBase<TaskeInsert> request);
        
        Task<ResponseBase<TaskeGet>> GetById(RequestBase<long> request);
        /*
        Task<ResponseBase<TaskeDelete>> Delete(RequestBase<TaskeDelete> request);
        Task<ResponseBase<TaskeUpdate>> Update(RequestBase<TaskeUpdate> request);
        Task<ResponseBase<List<TaskeGet>>> GetAll(RequestBase<string> request);
        */
    }
}
