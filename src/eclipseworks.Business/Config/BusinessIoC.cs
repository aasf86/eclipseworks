
using eclipseworks.Business.Contracts.UseCases.Project;
using eclipseworks.Business.Contracts.UseCases.Taske;
using eclipseworks.Business.UseCases.Project;
using eclipseworks.Business.UseCases.Taske;
using Microsoft.Extensions.DependencyInjection;

namespace eclipseworks.Business.Config
{
    public static class BusinessIoC
    {
        public static IServiceCollection AddBusinessIoC(this IServiceCollection services)
        {
            services.AddScoped<IProjectUseCase, ProjectUseCase>();
            services.AddScoped<ITaskeUseCase, TaskeUseCase>();

            return services;
        }
    }
}
