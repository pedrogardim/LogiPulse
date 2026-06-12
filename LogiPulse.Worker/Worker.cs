using System.Text.Json;
using System.Text.Json.Serialization;
using LogiPulse.Domain.Entities.Dispatches;

namespace LogiPulse.Worker;

public class Worker(
    ILogger<Worker> logger,
    IServiceScopeFactory scopeFactory
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true, // pretty-print
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
        };


        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();

            var dispatchRepository = scope.ServiceProvider.GetRequiredService<IDispatchRepository>();

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            var list = await dispatchRepository.ListAsync(null, 1, 100, stoppingToken);
            var dto = list.Select(d => new
            {
                Id = d.Id,
                TenantId = d.TenantId,
                Status = d.CurrentStatus,
                OriginFacilityId = d.OriginFacilityId,
                DestinationFacilityId = d.DestinationFacilityId,
                CreatedAtUtc = d.CreatedAtUtc
            }).ToList();
            var json = JsonSerializer.Serialize(dto, options);
            logger.LogInformation(json);
            await Task.Delay(5000, stoppingToken);
        }
    }
}