using FlowOps.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowOps.Infrastructure.Persistence;
public class FlowOpsDbContext : DbContext
{
    public FlowOpsDbContext(
        DbContextOptions<FlowOpsDbContext> options)
        : base(options)
    {
    }

    public DbSet<WorkItem> WorkItems => Set<WorkItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<WorkItem>(entity =>
        {
            entity.ToTable("WorkItems");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

            entity.Property(x => x.Description)
            .HasMaxLength(2000);

            entity.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

            entity.Property(x => x.Priority)
            .HasConversion<int>()
            .IsRequired();

            entity.Property(x => x.CreatedAt)
            .IsRequired();

            entity.Property(x => x.UpdatedAt)
            .IsRequired(false);
        });
    }
}
