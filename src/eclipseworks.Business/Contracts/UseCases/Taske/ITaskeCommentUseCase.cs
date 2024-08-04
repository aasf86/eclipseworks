using eclipseworks.Business.Dtos;
using eclipseworks.Business.Dtos.Taske;

namespace eclipseworks.Business.Contracts.UseCases.Taske
{
    public interface ITaskeCommentUseCase : IValidators
    {        
        Task<ResponseBase<long>> Insert(RequestBase<TaskeCommentInsert> request);        
        Task<ResponseBase<TaskeCommentGet>> GetById(RequestBase<long> request);
        Task<ResponseBase<bool>> Update(RequestBase<TaskeCommentUpdate> request);        
        Task<ResponseBase<bool>> Delete(RequestBase<TaskeCommentDelete> request);        
        Task<ResponseBase<List<TaskeCommentGet>>> GetAll(RequestBase<string> request);        
    }
}
