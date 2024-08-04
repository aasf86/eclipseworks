using eclipseworks.Business.Dtos;
using eclipseworks.Business.Dtos.Taske;

namespace eclipseworks.Business.Contracts.UseCases.Taske
{
    public interface ITaskeCommentUseCase : IValidators
    {        
        Task<ResponseBase<long>> Insert(RequestBase<TaskeInsert> request);        
        Task<ResponseBase<TaskeGet>> GetById(RequestBase<long> request);
        Task<ResponseBase<bool>> Update(RequestBase<TaskeUpdate> request);        
        Task<ResponseBase<bool>> Delete(RequestBase<TaskeDelete> request);        
        Task<ResponseBase<List<TaskeGet>>> GetAll(RequestBase<string> request);        
    }
}
