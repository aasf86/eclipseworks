using eclipseworks.Business.Contracts.UseCases.Taske;
using eclipseworks.Business.Dtos;
using eclipseworks.Business.Dtos.Taske;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty taskes, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace eclipseworks.Api.Controllers
{
    /// <summary>
    /// Controller para gestão de comentários.
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class TaskeCommentController : ControllerBase
    {
        private readonly ITaskeCommentUseCase _taskeCommentUseCase;
        private ITaskeCommentUseCase TaskeCommentUseCase => _taskeCommentUseCase;

        /// <summary>
        /// Controller para gestão de comentários.
        /// </summary>
        /// <param name="taskeCommentUseCase"></param>
        public TaskeCommentController(ITaskeCommentUseCase taskeCommentUseCase)
        {
            _taskeCommentUseCase = taskeCommentUseCase;
        }
        
        /// <summary>
        /// Inserir novo comentário.
        /// </summary>
        /// <param name="commentInsert"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] TaskeCommentInsert commentInsert)
        {
            if (ModelState.IsValid)
            {
                commentInsert.SetUserOwner(User?.FindAll(ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value ?? "");
//#if DEBUG
                commentInsert.SetUserOwner("aasf86@gmail.com");
//#endif
                var commentInsertRequest = RequestBase.New(commentInsert, "host:api", "1.0");
                var commentInsertResponse = await TaskeCommentUseCase.Insert(commentInsertRequest);

                if (commentInsertResponse.IsSuccess)
                    return Ok(commentInsertResponse);

                return BadRequest(commentInsertResponse);
            }

            return BadRequest();
        }

        /// <summary>
        /// Obter um comentário pelo 'Id'.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            if (id <= 0) return BadRequest(ResponseBase.New(new TaskeCommentGet(), Guid.NewGuid()));

            var commentGet = RequestBase.New(id, "host:api", "1.0");
            var commentGetResponse = await TaskeCommentUseCase.GetById(commentGet);

            if (commentGetResponse.IsSuccess)
                return Ok(commentGetResponse);

            return NotFound(commentGetResponse);
        }

        /// <summary>
        /// Atualizar o comentário.
        /// </summary>
        /// <param name="commentUpdate"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TaskeCommentUpdate commentUpdate)
        {
            if (ModelState.IsValid)
            {
                commentUpdate.SetUserEvent(User?.FindAll(ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value ?? "");
//#if DEBUG
                commentUpdate.SetUserEvent("aasf86@gmail.com");
//#endif
                var commentRequest = RequestBase.New(commentUpdate, "host:api", "1.0");
                var commentResponse = await TaskeCommentUseCase.Update(commentRequest);

                if (commentResponse.IsSuccess)
                    return Ok(commentResponse);

                return BadRequest(commentResponse);
            }
            return BadRequest();
        }

        /// <summary>
        /// Remove comentário pelo seu 'Id'.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var commentDelete = new TaskeCommentDelete { Id = id.ToString() };
            commentDelete.SetUserEvent(User?.FindAll(ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value ?? "");
//#if DEBUG
            commentDelete.SetUserEvent("aasf86@gmail.com");
//#endif
            var result = TaskeCommentUseCase.Validate(commentDelete);

            if (!result.IsSuccess) return BadRequest(ResponseBase.New(
                    commentDelete,
                    Guid.NewGuid(),
                    result.Validation.Select(x => x.ErrorMessage).ToList()
                )
            );

            var commentDeleteRequest = RequestBase.New(commentDelete, "host:api", "1.0");
            var commentDeleteResponse = await TaskeCommentUseCase.Delete(commentDeleteRequest);

            if (commentDeleteResponse.IsSuccess)
                return Ok(commentDeleteResponse);

            return BadRequest(commentDeleteResponse);
        }

        /// <summary>
        /// Procura comentários.
        /// </summary>
        /// <param name="filter">Para buscar todos insira '%'</param>
        /// <returns></returns>
        [HttpGet("all/{filter}")]
        public async Task<IActionResult> GetAll(string? filter = null)
        {
            if (string.IsNullOrWhiteSpace(filter)) return BadRequest(ResponseBase.New(new List<TaskeGet>(), Guid.NewGuid()));

            var commentGetAllRequest = RequestBase.New(filter, "host:api", "1.0");
            var commentGetResponse = await TaskeCommentUseCase.GetAll(commentGetAllRequest);

            if (commentGetResponse.IsSuccess)
                return Ok(commentGetResponse);

            return BadRequest(commentGetResponse);
        }
    }
}
