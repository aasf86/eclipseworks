using eclipseworks.Domain.Contracts.Repositories.Project;
using eclipseworks.Domain.Contracts.Repositories.Taske;
using eclipseworks.Infrastructure.EntitiesModels;
using eclipseworks.Infrastructure.Repositories.Project;
using eclipseworks.Infrastructure.Repositories.Taske;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;

namespace eclipseworks.Infrastructure.Config
{
    public static class InfrastructureIoC
    {
        public static IServiceCollection AddInfrastructureIoC(this IServiceCollection services, IConfigurationManager config)
        {
            services.AddScoped<IDbConnection>(src =>
            {
                AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                var connection = new NpgsqlConnection(config.GetConnectionString("Default"));
                return connection;
            });

            services.AddScoped<IProjectRepository<ProjectModel>, ProjectRepository>();
            services.AddScoped<ITaskeRepository<TaskeModel>, TaskeRepository>();
            services.AddScoped<ITaskeCommentRepository<TaskeCommentModel>, TaskeCommentRepository>();

            return services;
        }
    }
}
