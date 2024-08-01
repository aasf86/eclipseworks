using eclipseworks.Business.Contracts.UseCases.Project;
using eclipseworks.Business.Dtos;
using eclipseworks.Business.Dtos.Project;
using eclipseworks.Domain.Contracts.Repositories.Project;
using eclipseworks.Infrastructure.EntitiesModels;
using Microsoft.Extensions.Logging;
using System.Data;
using static eclipseworks.Domain.Entities.Project;
using Entity = eclipseworks.Domain.Entities;

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
                "Iniciando [Insert] do projeto: {Projeto}".LogInf(projectInsertRequest.Data.Name);

                var projectInsert = projectInsertRequest.Data;
                var projectInsertResponse = ResponseBase.New(projectInsert, projectInsertRequest.RequestId);
                var result = Validate(projectInsert);

                if (!result.IsSuccess)
                {
                    projectInsertResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                    var errors = string.Join("\n", projectInsertResponse.Errors.ToArray());
                    $"Projeto inválido '{{Projeto}}': {errors} ".LogWrn(projectInsert.Name);                    
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
                "Iniciando [Get] do projeto: {ProjectId}".LogInf(projectGetRequest.Data.Id);

                var projectGet = projectGetRequest.Data;
                var projectGetResponse = ResponseBase.New(projectGetRequest.Data, projectGetRequest.RequestId);
                var result = Validate(projectGet);

                if (!result.IsSuccess)
                {
                    projectGetResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                    var errors = string.Join("\n", projectGetResponse.Errors.ToArray());
                    $"Projeto inválido '{{ProjectId}}': {errors} ".LogWrn(projectGet.Id);
                    return projectGetResponse;
                }

                await UnitOfWorkExecute(async () =>
                {
                    var projectFromDb = await ProjectRepository.GetById(long.Parse(projectGet.Id));

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
        /*
                public async Task<ResponseBase<ProjectGet>> GetByPlate(RequestBase<ProjectGet> ProjectGetRequest)
                {
                    try
                    {
        #if !DEBUG
                        if (!IsInRole(RoleTypeeclipseworks.Admin)) throw new UnauthorizedAccessException();
        #endif

                        "Iniciando [GetByPlate] de motocicleta: {Plate}".LogInf(ProjectGetRequest.Data.Plate);                

                        var ProjectGet = ProjectGetRequest.Data;
                        var ProjectGetResponse = ResponseBase.New(ProjectGet, ProjectGetRequest.RequestId);
                        var result = Validate(ProjectGet);                

                        if (!result.IsSuccess)
                        {
                            ProjectGetResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                            var errors = string.Join("\n", ProjectGetResponse.Errors.ToArray());
                            $"Motocicleta inválida '{{Plate}}': {errors} ".LogWrn(ProjectGet.Plate);
                            return ProjectGetResponse;
                        }

                        await UnitOfWorkExecute(async () =>
                        {
                            var motocycleFromDb = await ProjectRepository.GetByPlate(ProjectGet.Plate);

                            if (motocycleFromDb is null)
                            {
                                ProjectGetResponse.Errors.Add(ProjectMsgDialog.NotFound);
                                $"{ProjectMsgDialog.NotFound} '{{Plate}}'".LogWrn();
                                return;
                            }

                            ProjectGetResponse.Data.Year = motocycleFromDb.Year;
                            ProjectGetResponse.Data.Model = motocycleFromDb.Model;
                            ProjectGetResponse.Data.Id = motocycleFromDb.Id;
                        });

                        return ProjectGetResponse;
                    }
                    catch (Exception exc)
                    {
                        "Erro ao [GetByPlate] motocicleta: {Plate}".LogErr(ProjectGetRequest.Data.Plate);
                        exc.Message.LogErr(exc);

                        var ProjectGetResponse = ResponseBase.New(ProjectGetRequest.Data, ProjectGetRequest.RequestId);
        #if DEBUG
                        ProjectGetResponse.Errors.Add(exc.Message);
        #endif
                        ProjectGetResponse.Errors.Add("Erro ao obter motocicleta.");

                        return ProjectGetResponse;                
                    }         
                }

                public async Task<ResponseBase<ProjectUpdate>> Update(RequestBase<ProjectUpdate> ProjectUpdateRequest)
                {
                    try
                    {
        #if !DEBUG
                        if (!IsInRole(RoleTypeeclipseworks.Admin)) throw new UnauthorizedAccessException();
        #endif

                        "Iniciando [Update] de motocicleta: {Plate}".LogInf(ProjectUpdateRequest.Data.Plate);

                        var ProjectUpdate = ProjectUpdateRequest.Data;
                        var ProjectUpdateResponse = ResponseBase.New(ProjectUpdate, ProjectUpdateRequest.RequestId);
                        var result = Validate(ProjectUpdate);

                        if (!result.IsSuccess)
                        {
                            ProjectUpdateResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                            var errors = string.Join("\n", ProjectUpdateResponse.Errors.ToArray());
                            $"Motocicleta inválida '{{Plate}}': {errors} ".LogWrn(ProjectUpdate.Plate);
                            return ProjectUpdateResponse;
                        }

                        await UnitOfWorkExecute(async () =>
                        {
                            var ProjectFromDb = await ProjectRepository.GetByPlate(ProjectUpdate.Plate, ProjectUpdate.Id);

                            if (ProjectFromDb is not null)
                            {
                                var strMsg = string.Format(ProjectMsgDialog.AlreadyRegistered, ProjectFromDb.Model);
                                ProjectUpdateResponse.Errors.Add(strMsg);
                                strMsg.LogWrn(ProjectFromDb?.Model);
                                return;
                            }

                            ProjectFromDb = await ProjectRepository.GetById(ProjectUpdate.Id);

                            if (ProjectFromDb is null)
                            {
                                ProjectUpdateResponse.Errors.Add(ProjectMsgDialog.NotFound);
                                $"{ProjectMsgDialog.NotFound} 'Id: {{Id}}'".LogWrn(ProjectUpdate.Id);
                                return;
                            }

                            var newProjectUpdate = new Entity.Project
                            (
                                ProjectFromDb.Year,
                                ProjectFromDb.Model,
                                ProjectUpdate.Plate
                            )
                            { Id = ProjectUpdate.Id };

                            await ProjectRepository.Update(newProjectUpdate);
                        });

                        return ProjectUpdateResponse;
                    }
                    catch (Exception exc)
                    {
                        "Erro ao [Update] motocicleta: {Plate}".LogErr(ProjectUpdateRequest.Data.Plate);
                        exc.Message.LogErr(exc);

                        var ProjectUpdateResponse = ResponseBase.New(ProjectUpdateRequest.Data, ProjectUpdateRequest.RequestId);
        #if DEBUG
                        ProjectUpdateResponse.Errors.Add(exc.Message);
        #endif
                        ProjectUpdateResponse.Errors.Add("Erro ao alterar motocicleta.");

                        return ProjectUpdateResponse;                
                    }
                }

                public async Task<ResponseBase<ProjectDelete>> Delete(RequestBase<ProjectDelete> ProjectDeleteRequest)
                {
                    try
                    {
        #if !DEBUG
                        if (!IsInRole(RoleTypeeclipseworks.Admin)) throw new UnauthorizedAccessException();
        #endif
                        "Iniciando [Delete] de motocicleta: {Id}".LogInf(ProjectDeleteRequest.Data.Id);

                        var ProjectDelete = ProjectDeleteRequest.Data;
                        var ProjectDeleteResponse = ResponseBase.New(ProjectDelete, ProjectDeleteRequest.RequestId);
                        var result = Validate(ProjectDelete);

                        if (!result.IsSuccess)
                        {
                            ProjectDeleteResponse.Errors.Add(ProjectMsgDialog.InvalidId);
                            return ProjectDeleteResponse;
                        }

                        await UnitOfWorkExecute(async () => 
                        {
                            var deleted = await ProjectRepository.Delete(ProjectDelete.Id);
                            if (!deleted)
                                ProjectDeleteResponse.Errors.Add(ProjectMsgDialog.NotFound);                    
                        });

                        return ProjectDeleteResponse;
                    }
                    catch (Exception exc)
                    {
                        "Erro [Delete] motocicleta: {Id}".LogErr(ProjectDeleteRequest.Data.Id);
                        exc.Message.LogErr(exc);

                        var ProjectDeleteResponse = ResponseBase.New(ProjectDeleteRequest.Data, ProjectDeleteRequest.RequestId);
        #if DEBUG
                        ProjectDeleteResponse.Errors.Add(exc.Message);
        #endif
                        ProjectDeleteResponse.Errors.Add("Erro ao excluir motocicleta.");

                        return ProjectDeleteResponse;
                    }             
                }
        */
    }
}
