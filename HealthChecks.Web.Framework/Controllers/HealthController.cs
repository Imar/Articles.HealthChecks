using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HealthChecks.Web.Framework.Health;

namespace HealthChecks.Web.Framework.Controllers
{
  [Route("api/health")]
  public class HealthController : ApiController
  {
    public async Task<IHttpActionResult> GetHealthInfo()
    {
      var result = new HealthCheckResult
      {
        MachineName = Environment.MachineName
      };

      var stopwatch = new Stopwatch();
      stopwatch.Start();
      var items = new List<HealthCheckItemResult>();
      var instances = new List<IHealthCheckProvider>();
      foreach (var provider in GetAllProviders())
      {
        var instance = (IHealthCheckProvider)Activator.CreateInstance(provider);
        instances.Add(instance);
      }
      await Task.WhenAll(instances.Select(async x => items.Add(await x.GetHealthCheckAsync())));
      stopwatch.Stop();
      result.TimeTakenInSeconds = stopwatch.Elapsed.TotalSeconds;
      result.HealthChecks.AddRange(items.OrderBy(x => x.SortOrder));
      return Ok(result);
    }

    private List<Type> GetAllProviders()
    {
      return GetTypesDeriving<IHealthCheckProvider>();
    }

    private static List<Type> GetTypesDeriving<T>()
    {
      return (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
              from assemblyType in domainAssembly.GetTypes()
              where typeof(T).IsAssignableFrom(assemblyType) && !assemblyType.IsAbstract
              select assemblyType).ToList();
    }
  }
}
