using System.Threading.Tasks;

namespace HealthChecks.Web.Framework.Health.Checks
{
  /// <summary>
  /// Provides health info about the main database.
  /// </summary>
  public class DatabaseHealthCheckProvider : IHealthCheckProvider
  {
    /// <summary>
    /// Tries to connect to the application's main database.
    /// </summary>
    public Task<HealthCheckItemResult> GetHealthCheckAsync()
    {
      var result = new HealthCheckItemResult(nameof(DatabaseHealthCheckProvider), SortOrder, "Checks the database", "Checks whether the main database can be accessed.");
      try
      {
        // Uncomment code below abd update it to match your DB context or make a direct SQL connection.
        //var context = new MyContext();
        //var item = await context.SomeSet.FirstAsync();
        //if (item != null)
        //{
        //    result.HealthState = HealthState.Healthy;
        //    result.Messages.Add("Successfully retrieved a record from the main database.");
        //}
        //else
        //{
        //    result.HealthState = HealthState.Unhealthy;
        //    result.Messages.Add("Connected to the database but could not find the requested record.");
        //}
      }
      catch
      {
        result.HealthState = HealthState.Unhealthy;
        result.Messages.Add("Error retrieving a record from the main database.");
      }
      return Task.FromResult(result);
    }

    /// <summary>
    /// Defines the order of this provider in the results.
    /// </summary>
    public int SortOrder { get; } = 20;
  }
}