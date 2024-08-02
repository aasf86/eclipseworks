using eclipseworks.Infrastructure.Config;
using eclipseworks.Business.Config;
using eclipseworks.Api.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen_eclipseworks();

//Inje��o de depend�ncias do eclipsworks
builder.Services.AddBusinessIoC();
builder.Services.AddInfrastructureIoC(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment()) //==> Deixei Swagger visivel para apresenta��o/demonstra��o
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
