using eclipseworks.Business.Contracts.UseCases.Project;
using eclipseworks.Business.Dtos;
using eclipseworks.Business.Dtos.Project;
using eclipseworks.Domain.Contracts.Repositories.Project;
using eclipseworks.Infrastructure.EntitiesModels;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data;
using static eclipseworks.Domain.Entities.Project;
using static eclipseworks.Infrastructure.Repositories.DocPostgresql;

namespace eclipseworks.Business.UseCases.Project
{
    public class ProjectUseCase : UseCaseBase, IProjectUseCase
    {
        private readonly IProjectRepository<ProjectModel> _projectRepository;
        private IProjectRepository<ProjectModel> ProjectRepository => _projectRepository;        

        public ProjectUseCase(
            ILogger<ProjectUseCase> logger, 
            IProjectRepository<ProjectModel> projectRepository,
            IDbConnection dbConnection) : base(logger, dbConnection) 
        {
            _projectRepository = projectRepository;            
            TransactionAssigner.Add(ProjectRepository.SetTransaction);
        }

        public async Task<ResponseBase<ProjectInsert>> Insert(RequestBase<ProjectInsert> projectInsertRequest)
        {
            try
            {
                var projectInsert = projectInsertRequest.Data;
                var projectInsertResponse = ResponseBase.New(projectInsert, projectInsertRequest.RequestId);
                var result = Validate(projectInsert);

                if (!result.IsSuccess)
                {
                    projectInsertResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                    var errors = string.Join("\n", projectInsertResponse.Errors.ToArray());                    
                    return projectInsertResponse;
                }

                if (string.IsNullOrEmpty(projectInsert.UserOwner))
                {
                    projectInsertResponse.Errors.Add(ProjectMsgDialog.RequiredUserOwner);                    
                    return projectInsertResponse;
                }

                var projectEntity = new ProjectModel(projectInsert.Name, projectInsert.UserOwner);

                await UnitOfWorkExecute(async () =>
                {                    
                    //var ProjectFromDb = await ProjectRepository.GetByPlate(projectInsert.Plate);
                    //aasf86 verificar no já cadastrado

                    await ProjectRepository.Insert(projectEntity);
                    projectInsertResponse.Data.SetId(projectEntity.Id.ToString());
                });

                return projectInsertResponse;
            }
            catch (Exception exc)
            {
                "Erro no [Insert] projeto: {Projeto}".LogErr(projectInsertRequest.Data.Name);
                exc.Message.LogErr(exc);

                var projectInsertResponse = ResponseBase.New(projectInsertRequest.Data, projectInsertRequest.RequestId);
#if DEBUG
                projectInsertResponse.Errors.Add(exc.Message);
#endif
                projectInsertResponse.Errors.Add("Erro ao inserir projeto.");

                return projectInsertResponse;
            }            
        }

        public async Task<ResponseBase<ProjectGet>> GetById(RequestBase<ProjectGet> projectGetRequest)
        {
            try
            {
                var projectGet = projectGetRequest.Data;
                var projectGetResponse = ResponseBase.New(projectGetRequest.Data, projectGetRequest.RequestId);
                var result = Validate(projectGet);

                if (!result.IsSuccess)
                {
                    projectGetResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());                    
                    return projectGetResponse;
                }

                await UnitOfWorkExecute(async () =>
                {
                    var projectFromDb = await ProjectRepository.GetById(long.Parse(projectGet.Id));

                    if (projectFromDb is null) 
                    {
                        projectGetResponse.Errors.Add(ProjectMsgDialog.NotFound);
                        return;
                    }

                    projectGetResponse.Data = new ProjectGet 
                    { 
                        Id = projectFromDb.Id.ToString(),
                        Name = projectFromDb.Name,
                        UserOwner = projectFromDb.UserOwner
                    };
                });

                return projectGetResponse;
            }
            catch (Exception exc)
            {
                "Erro no [Get] projeto: {ProjectId}".LogErr(projectGetRequest.Data.Id);
                exc.Message.LogErr(exc);

                var projectGetResponse = ResponseBase.New(projectGetRequest.Data, projectGetRequest.RequestId);
#if DEBUG
                projectGetResponse.Errors.Add(exc.Message);
#endif
                projectGetResponse.Errors.Add("Erro ao obter projeto");

                return projectGetResponse;
            }
        }

        public async Task<ResponseBase<ProjectDelete>> Delete(RequestBase<ProjectDelete> projectDeleteRequest)
        {
            try
            {
                var projectDelete = projectDeleteRequest.Data;
                var projectDeleteResponse = ResponseBase.New(projectDelete, projectDeleteRequest.RequestId);
                var result = Validate(projectDelete);

                if (!result.IsSuccess)
                {
                    projectDeleteResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());                    
                    return projectDeleteResponse;
                }

                await UnitOfWorkExecute(async () =>
                {
                    var projectFromDb = await ProjectRepository.GetById(long.Parse(projectDelete.Id));

                    if(projectFromDb is null)
                    {
                        projectDeleteResponse.Errors.Add(ProjectMsgDialog.NotFound);
                        return;
                    }

                    projectFromDb.SetLastEventByUser(projectDelete.UserEvent);

                    await ProjectRepository.Update(projectFromDb);

                    await ProjectRepository.Delete(projectFromDb);                    
                });

                return projectDeleteResponse;
            }
            catch (PostgresException exc)
            {
                "Erro [Delete] projeto: {ProjectId}".LogErr(projectDeleteRequest.Data.Id);
                exc.Message.LogErr(exc);

                var projectDeleteResponse = ResponseBase.New(projectDeleteRequest.Data, projectDeleteRequest.RequestId);                
#if DEBUG
                projectDeleteResponse.Errors.Add(exc.Message);
#endif
                if (
                    exc.SqlState.Equals(CodeErrors.FOREIGN_KEY_VIOLATION, StringComparison.OrdinalIgnoreCase) 
                    && 
                    exc.TableName.Equals(TaskeModel.TableName, StringComparison.OrdinalIgnoreCase)
                   )
                {
                    projectDeleteResponse.Errors.Add("Erro ao excluir projeto pois encontra-se associado a tarefas.");
                    return projectDeleteResponse;
                }

                projectDeleteResponse.Errors.Add("Erro ao excluir projeto.");

                return projectDeleteResponse;
            }
            catch (Exception exc)
            {
                "Erro [Delete] projeto: {ProjectId}".LogErr(projectDeleteRequest.Data.Id);
                exc.Message.LogErr(exc);

                var projectDeleteResponse = ResponseBase.New(projectDeleteRequest.Data, projectDeleteRequest.RequestId);
#if DEBUG
                projectDeleteResponse.Errors.Add(exc.Message);
#endif
                projectDeleteResponse.Errors.Add("Erro ao excluir projeto.");

                return projectDeleteResponse;
            }
            
        }

        public async Task<ResponseBase<ProjectUpdate>> Update(RequestBase<ProjectUpdate> projectUpdateRequest)
        {
            try
            {
                "Iniciando [Update] de projeto: {ProjectId}".LogInf(projectUpdateRequest.Data.Id);

                var projectUpdate = projectUpdateRequest.Data;
                var projectUpdateResponse = ResponseBase.New(projectUpdate, projectUpdateRequest.RequestId);
                var result = Validate(projectUpdate);

                if (!result.IsSuccess)
                {
                    projectUpdateResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                    var errors = string.Join("\n", projectUpdateResponse.Errors.ToArray());
                    $"Projeto inválido '{{ProjectId}}': {errors} ".LogWrn(projectUpdate.Id);
                    return projectUpdateResponse;
                }

                await UnitOfWorkExecute(async () =>
                {
                    var  projectFromDb = await ProjectRepository.GetById(long.Parse(projectUpdate.Id));

                    if (projectFromDb is null)
                    {
                        projectUpdateResponse.Errors.Add(ProjectMsgDialog.NotFound);
                        $"{ProjectMsgDialog.NotFound} 'Id: {{Id}}'".LogWrn(projectUpdate.Id);
                        return;
                    }

                    projectFromDb.SetName(projectUpdate.Name);
                    projectFromDb.SetLastEventByUser(projectUpdate.UserEvent);                    

                    await ProjectRepository.Update(projectFromDb);
                });

                return projectUpdateResponse;
            }
            catch (Exception exc)
            {
                "Erro ao [Update] projeto: {ProjectId}".LogErr(projectUpdateRequest.Data.Id);
                exc.Message.LogErr(exc);

                var projectUpdateResponse = ResponseBase.New(projectUpdateRequest.Data, projectUpdateRequest.RequestId);
#if DEBUG
                projectUpdateResponse.Errors.Add(exc.Message);
#endif
                projectUpdateResponse.Errors.Add("Erro ao alterar projeto.");

                return projectUpdateResponse;
            }
        }

        public async Task<ResponseBase<List<ProjectGet>>> GetAll(RequestBase<string> projectGetRequest)
        {
            try
            {
                var projectGetResponse = ResponseBase.New(new List<ProjectGet>(), projectGetRequest.RequestId);

                await UnitOfWorkExecute(async () =>
                {
                    var listProjects = await ProjectRepository.GetAll(projectGetRequest.Data);

                    projectGetResponse.Data = listProjects.Select(x => new ProjectGet
                    {
                        Id = x.Id.ToString(),
                        Name = x.Name,
                        UserOwner = x.UserOwner
                    }).ToList();

                });

                return projectGetResponse;
            }
            catch (Exception exc)
            {
                "Erro no [GetAll] projetos: {ProjectName}".LogErr(projectGetRequest.Data);
                exc.Message.LogErr(exc);

                var projectGetResponse = ResponseBase.New(new List<ProjectGet>(), projectGetRequest.RequestId);
#if DEBUG
                projectGetResponse.Errors.Add(exc.Message);
#endif
                projectGetResponse.Errors.Add("Erro ao obter projetos");

                return projectGetResponse;
            }
        }
    }
}
