using eclipseworks.Business.Dtos;
using eclipseworks.Business.Dtos.Project;
using eclipseworks.Business.UseCases.Project;
using eclipseworks.Domain.Contracts.Repositories.Project;
using eclipseworks.Infrastructure.EntitiesModels;
using Microsoft.Extensions.Logging;
using Moq;
using System.Data;
using static eclipseworks.Domain.Entities.Project;
using static eclipseworks.Domain.Entities.Rules.GeneralRules;

namespace eclipseworks.Test.Unit.Project
{
    public class UnitTestProjectUseCase
    {
        private ProjectUseCase _projectUseCase;
        private readonly Mock<ILogger<ProjectUseCase>> _logger;
        private readonly Mock<IProjectRepository<ProjectModel>> _projectRepository;
        private readonly Mock<IDbConnection> _dbConnection;
        private readonly Mock<IDbTransaction> _dbTransaction;

        public UnitTestProjectUseCase()
        {
            _logger = new Mock<ILogger<ProjectUseCase>>();
            _projectRepository = new Mock<IProjectRepository<ProjectModel>>();
            _dbTransaction = new Mock<IDbTransaction>();
            _dbConnection = new Mock<IDbConnection>();
        }

        private void Setup()
        {
            _dbConnection
                .Setup(x => x.BeginTransaction())
                .Returns(_dbTransaction.Object);

            _projectRepository
                .Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(Task.Run<ProjectModel?>(() => new ProjectModel("Anderson", "eu@mim.vc") { Id = 1 }));

            _projectUseCase = new ProjectUseCase(
                _logger.Object,
                _projectRepository.Object,
                _dbConnection.Object
            );
        }

        private void Setup_Without_Model()
        {
            _dbConnection
                .Setup(x => x.BeginTransaction())
                .Returns(_dbTransaction.Object);

            _projectRepository
                .Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(Task.Run(() => default(ProjectModel)));

            _projectUseCase = new ProjectUseCase(
                _logger.Object,
                _projectRepository.Object,
                _dbConnection.Object
            );
        }

        private async Task<ProjectGet> GetProject()
        {
            var projectGet = new ProjectGet { Id = "1" };
            var projectGetRequest = RequestBase.New(projectGet);
            var projectGetResponse = await _projectUseCase.GetById(projectGetRequest);
            var projectFromDb = projectGetResponse.Data;
            return projectFromDb;
        }

        #region [Insert]

        [Fact(DisplayName = "[Insert] Inserir com sucesso")]
        public async Task Insert_Project_With_Success()
        {
            Setup();

            var projectInsert = new ProjectInsert
            {
                Name = "Anderson"
            }
            .SetUserOwner("eu@mim.com");

            var projectInsertRequest = RequestBase.New(projectInsert);

            var projectInsertResponse = await _projectUseCase.Insert(projectInsertRequest);

            Assert.True(projectInsertResponse.IsSuccess);
        }

        [Fact(DisplayName = "[Insert] Quando request estiver sem nome, retorne erro")]
        public async Task When_Insert_Project_Without_Name_Then_Return_As_Erro()
        {
            Setup();

            var projectInsert = new ProjectInsert
            {
                Name = ""
            }
            .SetUserOwner("eu@mim.com");

            var projectInsertRequest = RequestBase.New(projectInsert);

            var projectInsertResponse = await _projectUseCase.Insert(projectInsertRequest);

            Assert.False(projectInsertResponse.IsSuccess);
        }

        [Fact(DisplayName = "[Insert] Quando request estiver sem usuário, retorno erro")]
        public async Task When_Insert_Project_Without_User_Then_Return_As_Erro()
        {
            Setup();

            var projectInsert = new ProjectInsert
            {
                Name = "Anderson"
            };

            var projectInsertRequest = RequestBase.New(projectInsert);

            var projectInsertResponse = await _projectUseCase.Insert(projectInsertRequest);

            Assert.False(projectInsertResponse.IsSuccess);
        }

        #endregion

        #region [Update]

        [Fact(DisplayName = "[Update] Alterar com sucesso")]
        public async Task Update_Project_With_Success()
        {
            Setup();

            var projectFromDb = await GetProject();

            var projectUpdate = new ProjectUpdate
            {
                Id = projectFromDb.Id,
                Name = projectFromDb.Name
            }
            .SetUserEvent("eu@mim.com");

            var projectUpdateRequest = RequestBase.New(projectUpdate);

            var projectUpdateResponse = await _projectUseCase.Update(projectUpdateRequest);

            Assert.True(projectUpdateResponse.IsSuccess);
        }

        [Fact(DisplayName = "[Update] Quando request estiver sem nome, retorne erro")]
        public async Task When_Update_Project_Without_Name_Then_Return_As_Erro()
        {
            Setup();

            var projectFromDb = await GetProject();

            var projectUpdate = new ProjectUpdate
            {
                Id = projectFromDb.Id,
                Name = ""
            }
            .SetUserEvent("eu@mim.com");

            var projectUpdateRequest = RequestBase.New(projectUpdate);

            var projectUpdateResponse = await _projectUseCase.Update(projectUpdateRequest);

            Assert.False(projectUpdateResponse.IsSuccess);
        }

        [Fact(DisplayName = "[Update] Quando não encontrar id retornar com erro.")]
        public async Task When_Not_Fount_Item_Id_Then_Return_As_Erro()
        {
            Setup_Without_Model();

            var projectUpdate = new ProjectUpdate
            {
                Id = "1",
                Name = "Anderson"
            }
            .SetUserEvent("eu@mim.com");

            var projectUpdateRequest = RequestBase.New(projectUpdate);

            var projectUpdateResponse = await _projectUseCase.Update(projectUpdateRequest);

            Assert.False(projectUpdateResponse.IsSuccess);
        }

        #endregion

        #region [Delete]

        [Fact(DisplayName = "[Delete] Deletar com sucesso")]
        public async Task Delete_With_Success()
        {
            Setup();

            var projectDelete = new ProjectDelete
            {
                Id = "1"
            }
            .SetUserEvent("eu@mim.com");

            var projectDeleteRequest = RequestBase.New(projectDelete);

            var projectUpdateResponse = await _projectUseCase.Delete(projectDeleteRequest);

            Assert.True(projectUpdateResponse.IsSuccess);
        }

        [Fact(DisplayName = "[Delete] Quando Id inválido, retornar como erro")]
        public async Task When_Id_Invalid_Then_Returno_As_Success()
        {
            Setup();

            var projectDelete = new ProjectDelete
            {
                Id = "0"
            }
            .SetUserEvent("eu@mim.com");

            var projectDeleteRequest = RequestBase.New(projectDelete);

            var projectUpdateResponse = await _projectUseCase.Delete(projectDeleteRequest);

            Assert.False(projectUpdateResponse.IsSuccess);
            Assert.Contains(ProjectMsgDialog.InvalidId, projectUpdateResponse.Errors);
        }

        [Fact(DisplayName = "[Delete] Quando sem usuário, retornar como erro")]
        public async Task When_Without_UserEvent_Then_Return_As_Success()
        {
            Setup();

            var projectDelete = new ProjectDelete { Id = "1" };

            var projectDeleteRequest = RequestBase.New(projectDelete);

            var projectUpdateResponse = await _projectUseCase.Delete(projectDeleteRequest);

            Assert.False(projectUpdateResponse.IsSuccess);
            Assert.Contains(MsgDialog.RequiredUserEvent, projectUpdateResponse.Errors);
        }

        [Fact(DisplayName = "[Delete] Quando não encontrar projeto, retornar como erro")]
        public async Task When_Not_Found_Item_Then_Return_As_Success()
        {
            Setup_Without_Model();

            var projectDelete = new ProjectDelete { Id = "10000" }.SetUserEvent("eu@mim.vc");

            var projectDeleteRequest = RequestBase.New(projectDelete);

            var projectUpdateResponse = await _projectUseCase.Delete(projectDeleteRequest);

            Assert.False(projectUpdateResponse.IsSuccess);
            Assert.Contains(ProjectMsgDialog.NotFound, projectUpdateResponse.Errors);
        }

        #endregion

        #region [Get]

        [Fact(DisplayName = "[Get] Encontrar projeto com sucesso")]
        public async Task Found_Item_With_Success()
        {
            Setup();

            var projectGet = new ProjectGet { Id = "1" };
            var projectGetRequest = RequestBase.New(projectGet);
            var projectGetResponse = await _projectUseCase.GetById(projectGetRequest);

            Assert.True(projectGetResponse.IsSuccess);
            Assert.True(long.Parse(projectGetResponse.Data.Id) > 0);
        }

        [Fact(DisplayName = "[Get] Quando Id for inválido, retornar como erro")]
        public async Task When_Invalid_Id_Item_Then_Return_As_Error()
        {
            Setup();

            var projectGet = new ProjectGet { Id = "0" };
            var projectGetRequest = RequestBase.New(projectGet);
            var projectGetResponse = await _projectUseCase.GetById(projectGetRequest);

            Assert.False(projectGetResponse.IsSuccess);
            Assert.Contains(ProjectMsgDialog.InvalidId, projectGetResponse.Errors);
        }

        [Fact(DisplayName = "[Get] Quando não encontrar projeto, retornar como erro")]
        public async Task When_Not_Found_Item_Id_Then_Return_As_Success()
        {
            Setup_Without_Model();

            var projectGet = new ProjectGet { Id = "999999999999" };
            var projectGetRequest = RequestBase.New(projectGet);
            var projectGetResponse = await _projectUseCase.GetById(projectGetRequest);

            Assert.False(projectGetResponse.IsSuccess);
            Assert.Contains(ProjectMsgDialog.NotFound, projectGetResponse.Errors);
        }

        #endregion
    }
}