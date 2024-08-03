using eclipseworks.Business.Contracts.UseCases.Taske;
using eclipseworks.Business.Dtos;
using eclipseworks.Business.Dtos.Taske;
using eclipseworks.Domain.Contracts.Repositories.Taske;
using eclipseworks.Infrastructure.EntitiesModels;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;
using static eclipseworks.Domain.Entities.Taske;

namespace eclipseworks.Business.UseCases.Taske
{
    public class TaskeUseCase : UseCaseBase, ITaskeUseCase
    {
        private readonly ITaskeRepository<TaskeModel> _taskeRepository;
        private ITaskeRepository<TaskeModel> TaskeRepository => _taskeRepository;        

        public TaskeUseCase(
            ILogger<TaskeUseCase> logger, 
            ITaskeRepository<TaskeModel> taskeRepository,
            IDbConnection dbConnection) : base(logger, dbConnection) 
        {
            _taskeRepository = taskeRepository;            
            TransactionAssigner.Add(TaskeRepository.SetTransaction);
        }

        public async Task<ResponseBase<TaskeInsert>> Insert(RequestBase<TaskeInsert> taskeInsertRequest)
        {
            try
            {
                "Iniciando [Insert] do tarefa: {Title}".LogInf(taskeInsertRequest.Data.Title);

                var taskeInsert = taskeInsertRequest.Data;
                var taskeInsertResponse = ResponseBase.New(taskeInsert, taskeInsertRequest.RequestId);
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
                    //var TaskeFromDb = await TaskeRepository.GetByPlate(taskeInsert.Plate);
                    //aasf86 verificar no já cadastrado

                    await TaskeRepository.Insert(taskeEntity);
                    taskeInsertResponse.Data.SetId(taskeEntity.Id.ToString());
                });

                return taskeInsertResponse;
            }
            catch (Exception exc)
            {
                "Erro no [Insert] tarefa: {Title}".LogErr(taskeInsertRequest.Data.Title);
                exc.Message.LogErr(exc);

                var taskeInsertResponse = ResponseBase.New(taskeInsertRequest.Data, taskeInsertRequest.RequestId);
#if DEBUG
                taskeInsertResponse.Errors.Add(exc.Message);
#endif
                taskeInsertResponse.Errors.Add("Erro ao inserir tarefa.");

                return taskeInsertResponse;
            }            
        }
/*
        public async Task<ResponseBase<TaskeGet>> GetById(RequestBase<TaskeGet> taskeGetRequest)
        {
            try
            {
                "Iniciando [Get] do tarefa: {TaskeId}".LogInf(taskeGetRequest.Data.Id);

                var taskeGet = taskeGetRequest.Data;
                var taskeGetResponse = ResponseBase.New(taskeGetRequest.Data, taskeGetRequest.RequestId);
                var result = Validate(taskeGet);

                if (!result.IsSuccess)
                {
                    taskeGetResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());                    
                    return taskeGetResponse;
                }

                await UnitOfWorkExecute(async () =>
                {
                    var taskeFromDb = await TaskeRepository.GetById(long.Parse(taskeGet.Id));

                    if (taskeFromDb is null) 
                    {
                        taskeGetResponse.Errors.Add(TaskeMsgDialog.NotFound);
                        return;
                    }

                    taskeGetResponse.Data = new TaskeGet 
                    { 
                        Id = taskeFromDb.Id.ToString(),
                        Name = taskeFromDb.Name,
                        UserOwner = taskeFromDb.UserOwner
                    };
                });

                return taskeGetResponse;
            }
            catch (Exception exc)
            {
                "Erro no [Get] tarefa: {TaskeId}".LogErr(taskeGetRequest.Data.Id);
                exc.Message.LogErr(exc);

                var taskeGetResponse = ResponseBase.New(taskeGetRequest.Data, taskeGetRequest.RequestId);
#if DEBUG
                taskeGetResponse.Errors.Add(exc.Message);
#endif
                taskeGetResponse.Errors.Add("Erro ao obter tarefa");

                return taskeGetResponse;
            }
        }

        public async Task<ResponseBase<TaskeDelete>> Delete(RequestBase<TaskeDelete> taskeDeleteRequest)
        {
            try
            {
                "Iniciando [Delete] do tarefa: {TaskeId}".LogInf(taskeDeleteRequest.Data.Id);

                var taskeDelete = taskeDeleteRequest.Data;
                var taskeDeleteResponse = ResponseBase.New(taskeDelete, taskeDeleteRequest.RequestId);
                var result = Validate(taskeDelete);

                if (!result.IsSuccess)
                {
                    taskeDeleteResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());                    
                    return taskeDeleteResponse;
                }

                await UnitOfWorkExecute(async () =>
                {
                    var taskeFromDb = await TaskeRepository.GetById(long.Parse(taskeDelete.Id));

                    if(taskeFromDb is null)
                    {
                        taskeDeleteResponse.Errors.Add(TaskeMsgDialog.NotFound);
                        return;
                    }

                    taskeFromDb.SetLastEventByUser(taskeDelete.UserEvent);

                    await TaskeRepository.Update(taskeFromDb);

                    await TaskeRepository.Delete(taskeFromDb);                    
                });

                return taskeDeleteResponse;
            }
            catch (Exception exc)
            {
                "Erro [Delete] tarefa: {TaskeId}".LogErr(taskeDeleteRequest.Data.Id);
                exc.Message.LogErr(exc);

                var taskeDeleteResponse = ResponseBase.New(taskeDeleteRequest.Data, taskeDeleteRequest.RequestId);
#if DEBUG
                taskeDeleteResponse.Errors.Add(exc.Message);
#endif
                taskeDeleteResponse.Errors.Add("Erro ao excluir tarefa.");

                return taskeDeleteResponse;
            }
        }

        public async Task<ResponseBase<TaskeUpdate>> Update(RequestBase<TaskeUpdate> taskeUpdateRequest)
        {
            try
            {
                "Iniciando [Update] de tarefa: {TaskeId}".LogInf(taskeUpdateRequest.Data.Id);

                var taskeUpdate = taskeUpdateRequest.Data;
                var taskeUpdateResponse = ResponseBase.New(taskeUpdate, taskeUpdateRequest.RequestId);
                var result = Validate(taskeUpdate);

                if (!result.IsSuccess)
                {
                    taskeUpdateResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                    var errors = string.Join("\n", taskeUpdateResponse.Errors.ToArray());
                    $"Tarefa inválido '{{TaskeId}}': {errors} ".LogWrn(taskeUpdate.Id);
                    return taskeUpdateResponse;
                }

                await UnitOfWorkExecute(async () =>
                {
                    var  taskeFromDb = await TaskeRepository.GetById(long.Parse(taskeUpdate.Id));

                    if (taskeFromDb is null)
                    {
                        taskeUpdateResponse.Errors.Add(TaskeMsgDialog.NotFound);
                        $"{TaskeMsgDialog.NotFound} 'Id: {{Id}}'".LogWrn(taskeUpdate.Id);
                        return;
                    }

                    taskeFromDb.SetName(taskeUpdate.Name);
                    taskeFromDb.SetLastEventByUser(taskeUpdate.UserEvent);                    

                    await TaskeRepository.Update(taskeFromDb);
                });

                return taskeUpdateResponse;
            }
            catch (Exception exc)
            {
                "Erro ao [Update] tarefa: {TaskeId}".LogErr(taskeUpdateRequest.Data.Id);
                exc.Message.LogErr(exc);

                var taskeUpdateResponse = ResponseBase.New(taskeUpdateRequest.Data, taskeUpdateRequest.RequestId);
#if DEBUG
                taskeUpdateResponse.Errors.Add(exc.Message);
#endif
                taskeUpdateResponse.Errors.Add("Erro ao alterar tarefa.");

                return taskeUpdateResponse;
            }
        }

        public async Task<ResponseBase<List<TaskeGet>>> GetAll(RequestBase<string> taskeGetRequest)
        {
            try
            {
                var taskeGetResponse = ResponseBase.New(new List<TaskeGet>(), taskeGetRequest.RequestId);

                await UnitOfWorkExecute(async () =>
                {
                    var listTaskes = await TaskeRepository.GetAll(taskeGetRequest.Data);

                    taskeGetResponse.Data = listTaskes.Select(x => new TaskeGet
                    {
                        Id = x.Id.ToString(),
                        Name = x.Name,
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
*/
    }
}
