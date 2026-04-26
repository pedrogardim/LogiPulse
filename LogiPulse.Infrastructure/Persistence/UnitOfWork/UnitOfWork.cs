using LogiPulse.Application.Interfaces;

namespace LogiPulse.Infrastructure.Persistence.UnitOfWork;

public class UnitOfWork(LogiPulseDbContext context) : IUnitOfWork
{
    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        context.Dispose();
    }
}