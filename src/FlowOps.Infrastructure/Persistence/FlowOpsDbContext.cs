using Microsoft.EntityFrameworkCore;

namespace FlowOps.Infrastructure.Persistence;
public class FlowOpsDbContext : DbContext
{
    public FlowOpsDbContext(
        DbContextOptions<FlowOpsDbContext> options)
        : base(options)
    {
    }
}
