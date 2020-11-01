using System.Threading.Tasks;

namespace HealthChecks.Web.Framework.Health
{
  /// <summary>
  /// Defines the interface for a health check provider.
  /// </summary>
  public interface IHealthCheckProvider
  {
    /// <summary>
    /// Returns the health heck info.
    /// </summary>
    Task<HealthCheckItemResult> GetHealthCheckAsync();

    /// <summary>
    /// Defines the order of this provider in the results.
    /// </summary>
    int SortOrder { get; }
  }
}