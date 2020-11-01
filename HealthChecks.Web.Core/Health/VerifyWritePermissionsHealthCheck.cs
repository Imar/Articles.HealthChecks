using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthChecks.Web.Core.Health
{
  public class VerifyWritePermissionsHealthCheck : IHealthCheck
  {
    private readonly string _folder;

    public VerifyWritePermissionsHealthCheck(string folder)
    {
      _folder = folder;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
      try
      {
        var fileName = Path.Combine(_folder, $"{Guid.NewGuid()}.txt");
        File.WriteAllText(fileName, "Test from health check");
        File.Delete(fileName);
        return Task.FromResult(HealthCheckResult.Healthy());
      }
      catch
      {
        return Task.FromResult(HealthCheckResult.Unhealthy());
      }
    }
  }
}
