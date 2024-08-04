using eclipseworks.Business.Auth;
using eclipseworks.Business.Contracts.UseCases.Taske;
using eclipseworks.Business.Dtos;
using eclipseworks.Business.Dtos.Taske;
using eclipseworks.Domain.Contracts.Repositories.Project;
using eclipseworks.Domain.Contracts.Repositories.Taske;
using eclipseworks.Domain.Entities.ValueObjects;
using eclipseworks.Infrastructure.EntitiesModels;
using Microsoft.Extensions.Logging;
using System.Data;
using static eclipseworks.Domain.Entities.Taske;

namespace eclipseworks.Business.UseCases.Taske
{
    public class TaskeUseCase : UseCaseBase, ITaskeUseCase
    {
        private readonly ITaskeRepository<TaskeModel> _taskeRepository;
        private ITaskeRepository<TaskeModel> TaskeRepository => _taskeRepository;

        private readonly IProjectRepository<ProjectModel> _projectRepository;
        private IProjectRepository<ProjectModel> ProjectRepository => _projectRepository;

        public TaskeUseCase(
            ILogger<TaskeUseCase> logger, 
            ITaskeRepository<TaskeModel> taskeRepository,
            IProjectRepository<ProjectModel> projectRepository,
            IDbConnection dbConnection) : base(logger, dbConnection) 
        {
            _taskeRepository = taskeRepository;
            _projectRepository = projectRepository;
            TransactionAssigner.Add(TaskeRepository.SetTransaction);
            TransactionAssigner.Add(ProjectRepository.SetTransaction);
        }

        public async Task<ResponseBase<long>> Insert(RequestBase<TaskeInsert> taskeInsertRequest)
        {
            try
            {
                var taskeInsert = taskeInsertRequest.Data;
                var taskeInsertResponse = ResponseBase.New((long)0, taskeInsertRequest.RequestId);
                var result = Validate(taskeInsert);

                if (!result.IsSuccess)
                {
                    taskeInsertResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                    var errors = string.Join("\n", taskeInsertResponse.Errors.ToArray());                    
                    return taskeInsertResponse;
                }

                if (string.IsNullOrEmpty(taskeInsert.UserOwner))
                {
                    taskeInsertResponse.Errors.Add(TaskeMsgDialog.RequiredUser);
                    return taskeInsertResponse;
                }

                var taskeEntity = new TaskeModel(
                    taskeInsert.UserOwner,
                    taskeInsert.Title,
                    taskeInsert.Description,
                    taskeInsert.Expires,
                    taskeInsert.Status,
                    taskeInsert.Priority,
                    long.Parse(taskeInsert.ProjectId));

                await UnitOfWorkExecute(async () =>
                {
                    var projectFromDb = await ProjectRepository.GetById(taskeEntity.ProjectId);

                    if (projectFromDb is null)
                    {
                        taskeInsertResponse.Errors.Add(TaskeMsgDialog.InvalidProjectId);                        
                        return;
                    }

                    var limitReached = await TaskeRepository.TaskeLimitReached(taskeEntity.ProjectId, TaskeRule.MaximumNumberTaskesPerProject);

                    if (limitReached)
                    {
                        taskeInsertResponse.Errors.Add(string.Format(TaskeMsgDialog.LimitReached, TaskeRule.MaximumNumberTaskesPerProject));
                        return;
                    }

                    await TaskeRepository.Insert(taskeEntity);
                    taskeInsertResponse.Data = taskeEntity.Id;
                });

                return taskeInsertResponse;
            }
            catch (Exception exc)
            {
                "Erro no [Insert] tarefa: {Title}".LogErr(taskeInsertRequest.Data.Title);
                exc.Message.LogErr(exc);

                var taskeInsertResponse = ResponseBase.New((long)0, taskeInsertRequest.RequestId);
#if DEBUG
                taskeInsertResponse.Errors.Add(exc.Message);
#endif
                taskeInsertResponse.Errors.Add("Erro ao inserir tarefa.");

                return taskeInsertResponse;
            }            
        }

        public async Task<ResponseBase<TaskeGet>> GetById(RequestBase<long> taskeGetRequest)
        {
            try
            {
                var taskeId = taskeGetRequest.Data;
                var taskeGetResponse = ResponseBase.New(new TaskeGet(), taskeGetRequest.RequestId);

                if (taskeId <= 0) return taskeGetResponse;

                await UnitOfWorkExecute(async () =>
                {
                    var taskeFromDb = await TaskeRepository.GetById(taskeId);

                    if (taskeFromDb is null)
                    {
                        taskeGetResponse.Errors.Add(TaskeMsgDialog.NotFound);
                        return;
                    }

                    taskeGetResponse.Data = new TaskeGet
                    {
                        Id = taskeFromDb.Id,
                        ProjectId = taskeFromDb.ProjectId,
                        Title = taskeFromDb.Title,
                        Description = taskeFromDb.Description,
                        Expires = taskeFromDb.Expires,
                        Status = taskeFromDb.Status,
                        Priority = taskeFromDb.Priority,
                        UserOwner = taskeFromDb.UserOwner
                    };
                });

                return taskeGetResponse;
            }
            catch (Exception exc)
            {
                "Erro no [GetById] tarefa: {TaskeId}".LogErr(taskeGetRequest.Data);
                exc.Message.LogErr(exc);

                var taskeGetResponse = ResponseBase.New(new TaskeGet(), taskeGetRequest.RequestId);
#if DEBUG
                taskeGetResponse.Errors.Add(exc.Message);
#endif
                taskeGetResponse.Errors.Add("Erro ao obter tarefa");

                return taskeGetResponse;
            }
        }

        public async Task<ResponseBase<bool>> Update(RequestBase<TaskeUpdate> taskeUpdateRequest)
        {
            try
            {
                var taskeUpdate = taskeUpdateRequest.Data;
                var taskeUpdateResponse = ResponseBase.New(false, taskeUpdateRequest.RequestId);
                var result = Validate(taskeUpdate);

                if (!result.IsSuccess)
                {
                    taskeUpdateResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                    var errors = string.Join("\n", taskeUpdateResponse.Errors.ToArray());                    
                    return taskeUpdateResponse;
                }

                await UnitOfWorkExecute(async () =>
                {
                    var taskeFromDb = await TaskeRepository.GetById(long.Parse(taskeUpdate.Id));

                    if (taskeFromDb is null)
                    {
                        taskeUpdateResponse.Errors.Add(TaskeMsgDialog.NotFound);                        
                        return;
                    }

                    taskeFromDb.SetTitle(taskeUpdate.Title);
                    taskeFromDb.SetDescription(taskeUpdate.Description);
                    taskeFromDb.SetExpires(taskeUpdate.Expires);
                    taskeFromDb.SetStatus(taskeUpdate.Status);
                    taskeFromDb.SetLastEventByUser(taskeUpdate.UserEvent);                    

                    await TaskeRepository.Update(taskeFromDb);

                    taskeUpdateResponse.Data = true;
                });

                return taskeUpdateResponse;
            }
            catch (Exception exc)
            {
                "Erro ao [Update] tarefa: {TaskeId}".LogErr(taskeUpdateRequest.Data.Id);
                exc.Message.LogErr(exc);

                var taskeUpdateResponse = ResponseBase.New(false, taskeUpdateRequest.RequestId);
#if DEBUG
                taskeUpdateResponse.Errors.Add(exc.Message);
#endif
                taskeUpdateResponse.Errors.Add("Erro ao alterar tarefa.");

                return taskeUpdateResponse;
            }
        }

        public async Task<ResponseBase<bool>> Delete(RequestBase<TaskeDelete> taskeDeleteRequest)
        {
            try
            {
                var taskeDelete = taskeDeleteRequest.Data;
                var taskeDeleteResponse = ResponseBase.New(false, taskeDeleteRequest.RequestId);
                var result = Validate(taskeDelete);

                if (!result.IsSuccess)
                {
                    taskeDeleteResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                    return taskeDeleteResponse;
                }

                await UnitOfWorkExecute(async () =>
                {
                    var taskeFromDb = await TaskeRepository.GetById(long.Parse(taskeDelete.Id));

                    if (taskeFromDb is null)
                    {
                        taskeDeleteResponse.Errors.Add(TaskeMsgDialog.NotFound);
                        return;
                    }

                    taskeFromDb.SetLastEventByUser(taskeDelete.UserEvent);

                    await TaskeRepository.Update(taskeFromDb);

                    await TaskeRepository.Delete(taskeFromDb);

                    taskeDeleteResponse.Data = true;
                });

                return taskeDeleteResponse;
            }           
            catch (Exception exc)
            {
                "Erro [Delete] tarefa: {TaskeId}".LogErr(taskeDeleteRequest.Data.Id);
                exc.Message.LogErr(exc);

                var taskeDeleteResponse = ResponseBase.New(false, taskeDeleteRequest.RequestId);
#if DEBUG
                taskeDeleteResponse.Errors.Add(exc.Message);
#endif
                taskeDeleteResponse.Errors.Add("Erro ao excluir tarefa.");

                return taskeDeleteResponse;
            }
        }              

        public async Task<ResponseBase<List<TaskeGet>>> GetAll(RequestBase<string> taskeGetRequest)
        {
            try
            {
                var taskeGetResponse = ResponseBase.New(new List<TaskeGet>(), taskeGetRequest.RequestId);

                await UnitOfWorkExecute(async () =>
                {
                    var listTaskesFromDb = await TaskeRepository.GetAll(taskeGetRequest.Data);

                    taskeGetResponse.Data = listTaskesFromDb.Select(x => new TaskeGet
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Description = x.Description,
                        Expires = x.Expires,
                        Status = x.Status,
                        Priority = x.Priority,
                        ProjectId = x.ProjectId,                        
                        UserOwner = x.UserOwner
                    }).ToList();
                });

                return taskeGetResponse;
            }
            catch (Exception exc)
            {
                "Erro no [GetAll] tarefas: {TaskeName}".LogErr(taskeGetRequest.Data);
                exc.Message.LogErr(exc);

                var taskeGetResponse = ResponseBase.New(new List<TaskeGet>(), taskeGetRequest.RequestId);
#if DEBUG
                taskeGetResponse.Errors.Add(exc.Message);
#endif
                taskeGetResponse.Errors.Add("Erro ao obter tarefas");

                return taskeGetResponse;
            }
        }

        public async Task<ResponseBase<List<TaskeReport>>> GetReport()
        {
            try
            {
#warning Verifica se é gerente(RoleTypeClaim.Manager)
//#if !DEBUG
                //if (!IsInRole(RoleTypeClaim.Manager)) throw new UnauthorizedAccessException();
//#endif
                var taskeReportResponse = ResponseBase.New(new List<TaskeReport>());

                await UnitOfWorkExecute(async () =>
                {
                    var taskeReportFromDb = await TaskeRepository.GetReport(TaskeRule.DaysReport);

                    taskeReportResponse.Data = taskeReportFromDb;
                });

                return taskeReportResponse;
            }
            catch (Exception exc)
            {
                "Erro no [GetReport] tarefas".LogErr();
                exc.Message.LogErr(exc);

                var taskeGetResponse = ResponseBase.New(new List<TaskeReport>());
#if DEBUG
                taskeGetResponse.Errors.Add(exc.Message);
#endif
                taskeGetResponse.Errors.Add("Erro ao obter relatório de tarefas");

                return taskeGetResponse;
            }
        }

    }
}
