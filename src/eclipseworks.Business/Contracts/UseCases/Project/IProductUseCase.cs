using eclipseworks.Business.Dtos;
using eclipseworks.Business.Dtos.Project;

namespace eclipseworks.Business.Contracts.UseCases.Project
{
    public interface IProjectUseCase : IValidators
    {
        
        Task<ResponseBase<ProjectInsert>> Insert(RequestBase<ProjectInsert> request);
        Task<ResponseBase<ProjectGet>> GetById(RequestBase<ProjectGet> request);
        Task<ResponseBase<ProjectDelete>> Delete(RequestBase<ProjectDelete> request);
        Task<ResponseBase<ProjectUpdate>> Update(RequestBase<ProjectUpdate> request);
    }
}
