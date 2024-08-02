using Microsoft.OpenApi.Models;
using System.Reflection;

namespace eclipseworks.Api.Config
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerGen_eclipseworks(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {                
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }        
    }
}