using eclipseworks.Business.Contracts.UseCases.Taske;
using eclipseworks.Business.Dtos;
using eclipseworks.Business.Dtos.Taske;
using eclipseworks.Business.Dtos.Taske;
using eclipseworks.Business.UseCases.Taske;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty taskes, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace eclipseworks.Api.Controllers
{
    /// <summary>
    /// Controller para gestão de cadastros de tarefas.
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class TaskeController : ControllerBase
    {
        private readonly ITaskeUseCase _taskeUseCase;
        private ITaskeUseCase TaskeUseCase => _taskeUseCase;

        /// <summary>
        /// Controller para gestão de cadastros de tarefas.
        /// </summary>
        /// <param name="taskeUseCase"></param>
        public TaskeController(ITaskeUseCase taskeUseCase)
        {
            _taskeUseCase = taskeUseCase;
        }
        
        /// <summary>
        /// Inserir nova tarefa.
        /// </summary>
        /// <param name="taskeInsert"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] TaskeInsert taskeInsert)
        {
            if (ModelState.IsValid)
            {
                if (User is not null) Thread.CurrentPrincipal = new ClaimsPrincipal(User.Identity);

                taskeInsert.SetUserOwner(User?.FindAll(ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value ?? "");
//#if DEBUG
                taskeInsert.SetUserOwner("aasf86@gmail.com");
//#endif
                var TaskeInsertRequest = RequestBase.New(taskeInsert, "host:api", "1.0");
                var TaskeInsertResponse = await TaskeUseCase.Insert(TaskeInsertRequest);

                if (TaskeInsertResponse.IsSuccess)
                    return Ok(TaskeInsertResponse);

                return BadRequest(TaskeInsertResponse);
            }

            return BadRequest();
        }

        /// <summary>
        /// Obter uma tarefa pelo 'Id'.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            if (id <= 0) return BadRequest(ResponseBase.New(new TaskeGet(), Guid.NewGuid()));

            var taskeGetRequest = RequestBase.New(id, "host:api", "1.0");
            var taskeGetResponse = await TaskeUseCase.GetById(taskeGetRequest);

            if (taskeGetResponse.IsSuccess)
                return Ok(taskeGetResponse);

            return NotFound(taskeGetResponse);
        }

        /// <summary>
        /// Atualizar o nome do tarefa.
        /// </summary>
        /// <param name="taskeUpdate"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TaskeUpdate taskeUpdate)
        {
            if (ModelState.IsValid)
            {
                if (User is not null) Thread.CurrentPrincipal = new ClaimsPrincipal(User.Identity);

                taskeUpdate.SetUserEvent(User?.FindAll(ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value ?? "");
//#if DEBUG
                taskeUpdate.SetUserEvent("aasf86@gmail.com");
//#endif
                var taskeUpdateRequest = RequestBase.New(taskeUpdate, "host:api", "1.0");
                var taskeUpdateResponse = await TaskeUseCase.Update(taskeUpdateRequest);

                if (taskeUpdateResponse.IsSuccess)
                    return Ok(taskeUpdateResponse);

                return BadRequest(taskeUpdateResponse);
            }
            return BadRequest();
        }
    }
}
