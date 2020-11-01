using System.Collections.Generic;
using System.Linq;

namespace HealthChecks.Web.Framework.Health
{
  /// <summary>
  /// DTO for the health check endpoint to return base data as well as the info from the individual health check providers.
  /// </summary>
  public class HealthCheckResult
  {
    /// <summary>
    /// Indicates if at least one of the connected health checks is not considered healthy.
    /// </summary>
    public bool HasFailures
    {
      get { return HealthChecks.Any(x => x.HealthState != HealthState.Healthy); }
    }

    /// <summary>
    /// Gets or sets the current machine name.
    /// </summary>
    public string MachineName { get; set; }

    /// <summary>
    /// Gets or sets the total time in seconds it took to test all connected health check providers.
    /// </summary>
    public double TimeTakenInSeconds { get; set; }

    /// <summary>
    /// Gets the information of the connected health check providers.
    /// </summary>
    public List<HealthCheckItemResult> HealthChecks { get; } = new List<HealthCheckItemResult>();
  }
}