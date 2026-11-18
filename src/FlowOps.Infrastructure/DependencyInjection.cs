using FlowOps.Application.Interfaces;
using FlowOps.Infrastructure.Persistence;
using FlowOps.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlowOps.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Database connection string is missing.");

            services.AddDbContext<FlowOpsDbContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<IWorkItemRepository, WorkItemRepository>();

            return services;
        }
    }
}
