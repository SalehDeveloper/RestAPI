using Microsoft.Extensions.Diagnostics.HealthChecks;
using Movies.Application.Data;

namespace Movies.Api.Health
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DatabaseHealthCheck> _logger;
        public const string Name = "Database";
        public DatabaseHealthCheck(ApplicationDbContext context, ILogger<DatabaseHealthCheck> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = new())
        {
          try
            {
               var canConnect = await _context.Database.CanConnectAsync(cancellationToken);

                if (canConnect)
                    return HealthCheckResult.Healthy("Database is reachable");
                return HealthCheckResult.Unhealthy("Database in not reachable");
                
            }
            catch(Exception ex )
            {
                _logger.LogError(ex, "Database health check failed.");
                return HealthCheckResult.Unhealthy("Database check threw an exception", ex);
            }
        }
    }
}
