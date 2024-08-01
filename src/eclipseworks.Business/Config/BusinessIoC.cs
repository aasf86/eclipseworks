
using eclipseworks.Business.Contracts.UseCases.Project;
using eclipseworks.Business.UseCases.Project;
using Microsoft.Extensions.DependencyInjection;

namespace eclipseworks.Business.Config
{
    public static class BusinessIoC
    {
        public static IServiceCollection AddBusinessIoC(this IServiceCollection services)
        {
            services.AddScoped<IProjectUseCase, ProjectUseCase>();

            return services;
        }
    }
}
