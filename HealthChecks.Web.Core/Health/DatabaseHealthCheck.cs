using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthChecks.Web.Core.Health
{
  public class DatabaseHealthCheck : IHealthCheck
  {
    private readonly IConfiguration _configuration;

    public DatabaseHealthCheck(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
      try
      {
        var connection = new SqlConnection(_configuration.GetConnectionString("ContentContext"));
        await connection.OpenAsync(cancellationToken);
        var command = new SqlCommand("SELECT TOP 1 * FROM Content", connection);
        var reader = await command.ExecuteReaderAsync(cancellationToken);
        var canRead = await reader.ReadAsync(cancellationToken);
        connection.Close();
        if (!canRead)
        {
          return HealthCheckResult.Unhealthy();
        }
        return HealthCheckResult.Healthy();
      }
      catch
      {
        return HealthCheckResult.Unhealthy();
      }
    }
  }
}
