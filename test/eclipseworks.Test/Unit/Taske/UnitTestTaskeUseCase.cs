using eclipseworks.Business.Dtos;
using eclipseworks.Business.Dtos.Taske;
using eclipseworks.Business.UseCases.Taske;
using eclipseworks.Domain.Contracts.Repositories.Project;
using eclipseworks.Domain.Contracts.Repositories.Taske;
using eclipseworks.Infrastructure.EntitiesModels;
using eclipseworks.Infrastructure.Repositories.Taske;
using Microsoft.Extensions.Logging;
using Moq;
using System.Data;
using static eclipseworks.Domain.Entities.Project;
using static eclipseworks.Domain.Entities.Taske;

namespace eclipseworks.Test.Unit.Taske
{
    public class UnitTestTaskeUseCase
    {
        private TaskeUseCase _taskeUseCase;
        private readonly Mock<ILogger<TaskeUseCase>> _logger;
        private readonly Mock<ITaskeRepository<TaskeModel>> _taskeRepository;
        private readonly Mock<IProjectRepository<ProjectModel>> _projectRepository;
        private readonly Mock<IDbConnection> _dbConnection;
        private readonly Mock<IDbTransaction> _dbTransaction;

        private int _countCheckLimited = 0;

        public UnitTestTaskeUseCase()
        {            
            _logger = new Mock<ILogger<TaskeUseCase>>();
            _taskeRepository = new Mock<ITaskeRepository<TaskeModel>>();
            _projectRepository = new Mock<IProjectRepository<ProjectModel>>();
            _dbTransaction = new Mock<IDbTransaction>();
            _dbConnection = new Mock<IDbConnection>();            
        }
        
        private void Setup()
        {
            _dbConnection
                .Setup(x => x.BeginTransaction())
                .Returns(_dbTransaction.Object);

            _taskeRepository
                .Setup(x => x.TaskeLimitReached(1, TaskeRule.MaximumNumberTaskesPerProject))
                .Returns(Task.Run(() => false));                

            /*
                .Returns(Task.Run<bool>(() =>
                {
                    _countCheckLimited += 1;
                    return _countCheckLimited == TaskeRule.MaximumNumberTaskesPerProject;
                }))             
             */

            _projectRepository
                .Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(Task.Run<ProjectModel?>(() => new ProjectModel("Anderson", "eu@mim.vc") { Id = 1 }));
            
            _taskeUseCase = new TaskeUseCase(
                _logger.Object,
                _taskeRepository.Object,
                _projectRepository.Object,
                _dbConnection.Object
            );
        }        

        [Fact(DisplayName = "[Insert] Apos inserir tarefas, não será permitido novos objetos.")]
        public async Task When_Insert_Amount_Itens_Defined_Then_Not_Allowed_New_Objects()
        {
            Setup();

            var newTaske = new TaskeInsert
            {
                Title = "Titulo",
                Description = "Description",
                Expires = DateTime.UtcNow,
                Priority = ePriority.Low,
                Status = eStatus.Pending,
                ProjectId = "1"
            }.SetUserOwner("eu@mim.vc");

            var taskeInsertRequest = RequestBase.New(newTaske, "unit-test", "1");

            for (int i = 0; i < TaskeRule.MaximumNumberTaskesPerProject; i++)
            {
                var result = await _taskeUseCase.Insert(taskeInsertRequest);

                if (result.IsSuccess) _countCheckLimited++;
            }

            if (_countCheckLimited == TaskeRule.MaximumNumberTaskesPerProject)
            {
                _taskeRepository
                    .Setup(x => x.TaskeLimitReached(1, TaskeRule.MaximumNumberTaskesPerProject))
                    .Returns(Task.Run(() => true));
            }            

            var taskeInsertResponse = await _taskeUseCase.Insert(taskeInsertRequest);            

            Assert.False(taskeInsertResponse.IsSuccess);
            Assert.Contains(string.Format(TaskeMsgDialog.LimitReached, TaskeRule.MaximumNumberTaskesPerProject), taskeInsertResponse.Errors);            
        }        
    }
}
