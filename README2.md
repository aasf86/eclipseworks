# [Fase 1](README.md)
# [Fase 2](README2.md)
## Desafio backend *[.NET (C#)](https://meteor-ocelot-f0d.notion.site/NET-C-5281edbec2e4480d98552e5ca0242c5b)*. 

### Sugestões
- Importante definir as caracteristidas das entidades para armazenamento.
    - Ex: Projeto=>Nome|literal(size=150)
- Definir permissões e perfis de acesso ao sistema.
- No caso do relatório, a verificação de acesso ficou comentada aguardando um IDP SSO OAuth 2.
    - Aqui será verificado a role 'RoleTypeClaim.Manager'
        ```csharp
            [Route("api/[controller]")]
            [ApiController]
            public class TaskeController : ControllerBase
            {
                /// <summary>
                /// Obtem dados para relatorio de tarefas por usuários nos ultimos 30 dias.
                /// </summary>        
                /// <returns></returns>
        #warning Verifica se é gerente(RoleTypeClaim.Manager)
        //#if !DEBUG
                //[Authorize(Roles = RoleTypeClaim.Manager)]
        //#endif
                [HttpGet("Report")]
                public async Task<IActionResult> GetReport()
                {
                    if (User is not null) Thread.CurrentPrincipal = new ClaimsPrincipal(User.Identity);

                    var reportResponse = await TaskeUseCase.GetReport();

                    if (reportResponse.IsSuccess)
                        return Ok(reportResponse);
                    
                    return BadRequest(reportResponse);
                }
            }
        ```
    - No TaskeUseCase.cs também será verificado a role 'RoleTypeClaim.Manager', caso seja usado via pacote nuget
        ```csharp
            public class TaskeUseCase
            {
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
        ```        

# [Fase 3](README3.md)
