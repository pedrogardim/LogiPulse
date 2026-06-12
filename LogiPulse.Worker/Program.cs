using LogiPulse.Application.Common;
using LogiPulse.Application.DependencyInjection;
using LogiPulse.Infrastructure.DependencyInjection;
using LogiPulse.Worker;
using LogiPulse.Worker.Context;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddScoped<IUserContext, WorkerContext>();

builder.Services
    .AddLogiPulseApplication()
    .AddLogiPulseInfrastructure(builder.Configuration);

var host = builder.Build();
host.Run();