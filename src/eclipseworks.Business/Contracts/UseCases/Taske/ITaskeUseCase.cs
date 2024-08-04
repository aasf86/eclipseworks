using eclipseworks.Business.Dtos;
using eclipseworks.Business.Dtos.Taske;
using eclipseworks.Domain.Entities.ValueObjects;

namespace eclipseworks.Business.Contracts.UseCases.Taske
{
    public interface ITaskeUseCase : IValidators
    {        
        Task<ResponseBase<long>> Insert(RequestBase<TaskeInsert> request);
        Task<ResponseBase<TaskeGet>> GetById(RequestBase<long> request);
        Task<ResponseBase<bool>> Update(RequestBase<TaskeUpdate> request);
        Task<ResponseBase<bool>> Delete(RequestBase<TaskeDelete> request);
        Task<ResponseBase<List<TaskeGet>>> GetAll(RequestBase<string> request);
        Task<ResponseBase<List<TaskeReport>>> GetReport();
    }
}
