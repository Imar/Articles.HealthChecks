using System.Linq;
using HealthChecks.Network.Core;
using HealthChecks.Web.Core.Mail;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace HealthChecks.Web.Core.Health
{
  public static class HealthCheckExtensions
  {
    public static IHealthChecksBuilder AddFileWritePermissionsCheck(this IHealthChecksBuilder builder, string folderToTest)
    {
      var check = new VerifyWritePermissionsHealthCheck(folderToTest);
      builder.AddCheck("Check folder write permissions", check);

      return builder;
    }

    public static IHealthChecksBuilder AddSmtpHealthCheck(this IHealthChecksBuilder builder, SmtpSettings smtpSettings)
    {
      builder.AddSmtpHealthCheck(setup =>
      {
        setup.Host = smtpSettings.MailServer;
        setup.Port = smtpSettings.Port;
        setup.ConnectionType = SmtpConnectionType.TLS;
        setup.LoginWith(smtpSettings.UserName, smtpSettings.Password);
        setup.AllowInvalidRemoteCertificates = true;
      }, tags: new[] { "smtp" }, failureStatus: HealthStatus.Degraded);

      return builder;
    }

    public static IHealthChecksBuilder AddDatabaseCheck(this IHealthChecksBuilder builder)
    {
      builder.AddCheck<DatabaseHealthCheck>("SomeOtherCheck");
      return builder;
    }

    public static void MapHealthChecksWithJsonResponse(this IEndpointRouteBuilder endpoints, PathString path)
    {
      var options = new HealthCheckOptions
      {
        ResponseWriter = async (httpContext, healthReport) =>
        {
          httpContext.Response.ContentType = "application/json";

          var result = JsonConvert.SerializeObject(new
          {
            status = healthReport.Status.ToString(),
            totalDurationInSeconds = healthReport.TotalDuration.TotalSeconds,
            entries = healthReport.Entries.Select(e => new
            {
              key = e.Key,
              status = e.Value.Status.ToString(),
              description = e.Value.Description,
              data = e.Value.Data,
              exception = e.Value.Exception
            })
          });
          await httpContext.Response.WriteAsync(result);
        }
      };
      endpoints.MapHealthChecks(path, options);
    }
  }
}