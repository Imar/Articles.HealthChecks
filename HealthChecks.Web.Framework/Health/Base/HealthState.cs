namespace HealthChecks.Web.Framework.Health
{
  /// <summary>
  /// Defines the various health states.
  /// </summary>
  public enum HealthState
  {
    /// <summary>
    /// The resource is considered up and healthy.
    /// </summary>
    Healthy,

    /// <summary>
    /// The resource is considered to be degraded which means it's still up but not fully functional.
    /// </summary>
    Degraded,

    /// <summary>
    /// The resource is considered to be unhealthy.
    /// </summary>
    Unhealthy
  }
}