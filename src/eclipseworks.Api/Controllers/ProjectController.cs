using eclipseworks.Business.Contracts.UseCases.Project;
using eclipseworks.Business.Dtos;
using eclipseworks.Business.Dtos.Project;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace eclipseworks.Api.Controllers
{
    /// <summary>
    /// Controller para gestão de cadastros de projetos.
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectUseCase _ProjectUseCase;
        private IProjectUseCase ProjectUseCase => _ProjectUseCase;

        /// <summary>
        /// Controller para gestão de cadastros de projetos.
        /// </summary>
        /// <param name="ProjectUseCase"></param>
        public ProjectController(IProjectUseCase ProjectUseCase)
        {
            _ProjectUseCase = ProjectUseCase;
        }
        
        /// <summary>
        /// Inserir novo projeto.
        /// </summary>
        /// <param name="projectInsert"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] ProjectInsert projectInsert)
        {
            if (ModelState.IsValid)
            {
                if (User is not null) Thread.CurrentPrincipal = new ClaimsPrincipal(User.Identity);

                projectInsert.SetUserOwner(User?.FindAll(ClaimTypes.NameIdentifier)?.FirstOrDefault()?.Value ?? "");
#if DEBUG
                projectInsert.SetUserOwner("aasf86@gmail.com");
#endif
                var ProjectInsertRequest = RequestBase.New(projectInsert, "host:api", "1.0");
                var ProjectInsertResponse = await ProjectUseCase.Insert(ProjectInsertRequest);

                if (ProjectInsertResponse.IsSuccess)
                    return Ok(ProjectInsertResponse);

                return BadRequest(ProjectInsertResponse);
            }

            return BadRequest();
        }

        /// <summary>
        /// Obter um projeto pelo 'Id'.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var projectGet = new ProjectGet { Id = id.ToString() };
            var result = ProjectUseCase.Validate(projectGet);

            if (!result.IsSuccess) return BadRequest(result);

            var projectGetRequest = RequestBase.New(projectGet, "host:api", "1.0");
            var projectGetResponse = await ProjectUseCase.GetById(projectGetRequest);

            if (projectGetResponse.IsSuccess && projectGetResponse.Data.Id != "")
                return Ok(projectGetResponse);

            return NotFound(projectGetResponse);
        }
        /*                
                /// <summary>
                /// Obter uma projeto pela sua placa.
                /// </summary>
                /// <param name="plate"></param>
                /// <returns></returns>
                [HttpGet("{plate}")]
                public async Task<IActionResult> GetByPlate(string plate)
                {
                    var ProjectGet = new ProjectGet { Plate = plate };
                    var result = ProjectUseCase.Validate(ProjectGet);

                    if (!result.IsSuccess) return BadRequest(result);

                    var ProjectGetRequest = RequestBase.New(ProjectGet, "host:api", "1.0");
                    var ProjectGetResponse = await ProjectUseCase.GetByPlate(ProjectGetRequest);

                    if (ProjectGetResponse.IsSuccess && ProjectGetResponse.Data.Id > 0) 
                        return Ok(ProjectGetResponse);

                    return NotFound(ProjectGetResponse);
                }

                /// <summary>
                /// Atualizar a placa de uma projeto.
                /// </summary>
                /// <param name="Project"></param>
                /// <returns></returns>
                [HttpPut]
                public async Task<IActionResult> Update([FromBody] ProjectUpdate Project)
                {
                    if (ModelState.IsValid)
                    {
                        var ProjectUpdateRequest = RequestBase.New(Project, "host:api", "1.0");
                        var ProjectUpdateResponse = await ProjectUseCase.Update(ProjectUpdateRequest);

                        if (ProjectUpdateResponse.IsSuccess)
                            return Ok(ProjectUpdateResponse);

                        return BadRequest(ProjectUpdateResponse);
                    }
                    return BadRequest();
                }

                /// <summary>
                /// Remove projeto pelo seu 'Id'.
                /// </summary>
                /// <param name="id"></param>
                /// <returns></returns>
                [HttpDelete("{id}")]
                public async Task<IActionResult> Delete(int id)
                {
                    var ProjectDelete = new ProjectDelete { Id = id };
                    var result = ProjectUseCase.Validate(ProjectDelete);

                    if (!result.IsSuccess) return BadRequest(result);

                    var ProjectDeleteRequest = RequestBase.New(ProjectDelete, "host:api", "1.0");
                    var ProjectdeleteResponse = await ProjectUseCase.Delete(ProjectDeleteRequest);

                    if (ProjectdeleteResponse.IsSuccess)
                        return Ok(ProjectdeleteResponse);

                    return BadRequest(ProjectdeleteResponse);
                }     
        */
    }
}
