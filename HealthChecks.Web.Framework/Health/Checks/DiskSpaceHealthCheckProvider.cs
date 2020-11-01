using System;
using System.IO;
using System.Threading.Tasks;

namespace HealthChecks.Web.Framework.Health.Checks
{
  /// <summary>
  /// Provides health info about the file system.
  /// </summary>
  public class DiskSpaceHealthCheckProvider : IHealthCheckProvider
  {
    private const int MinPercentageFree = 10;
    private const int WarningPercentageFree = 20;

    /// <summary>
    /// Returns the health heck info.
    /// </summary>
    public Task<HealthCheckItemResult> GetHealthCheckAsync()
    {
      var result = new HealthCheckItemResult(nameof(DiskSpaceHealthCheckProvider), SortOrder, "Checks disk space", $"Validates that the available disk space is more than {MinPercentageFree} percent.");
      try
      {
        var percentageFree = GetPercentageFree("C");
        result.HealthState = DetermineState(percentageFree);
        result.Messages.Add($"There is {(percentageFree > MinPercentageFree ? "more" : "LESS")} than {MinPercentageFree} percent available disk space.");
      }
      catch
      {
        result.HealthState = HealthState.Unhealthy;
        result.Messages.Add("Could not validate disk space.");
      }
      return Task.FromResult(result);
    }

    private static double GetPercentageFree(string drive)
    {
      var driveInfo = new DriveInfo(drive);
      var freeSpace = driveInfo.TotalFreeSpace;
      var totalSpace = driveInfo.TotalSize;
      var percentageFree = Convert.ToDouble(freeSpace) / Convert.ToDouble(totalSpace) * 100;
      return percentageFree;
    }

    private static HealthState DetermineState(double percentageFree)
    {
      if (percentageFree < MinPercentageFree)
      {
        return  HealthState.Unhealthy;
      }
      if (percentageFree < WarningPercentageFree)
      {
        return HealthState.Degraded;
      }
      return HealthState.Healthy;
    }

    /// <summary>
    /// Defines the order of this provider in the results.
    /// </summary>
    public int SortOrder => 30;
  }
}