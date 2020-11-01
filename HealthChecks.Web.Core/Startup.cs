using System.IO;
using HealthChecks.UI.Client;
using HealthChecks.Web.Core.Health;
using HealthChecks.Web.Core.Mail;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace HealthChecks.Web.Core
{
  public class Startup
  {
    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
      _environment = environment;
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.Configure<SmtpSettings>(options => Configuration.GetSection("SmtpSettings")
          .Bind(options, c => c.BindNonPublicProperties = true));

      var smtpSettings = new SmtpSettings();
      Configuration.GetSection("SmtpSettings").Bind(smtpSettings, c => c.BindNonPublicProperties = true);

      var drives = DriveInfo.GetDrives();

      services.AddHealthChecks()
        .AddSqlServer(Configuration["ConnectionStrings:ContentContext"])
        .AddSmtpHealthCheck(smtpSettings)
        .AddDiskStorageHealthCheck(x => x.AddDrive("C:\\", 10_000), "Check primary disk - warning", HealthStatus.Degraded)
        .AddDiskStorageHealthCheck(x => x.AddDrive("C:\\", 2_000), "Check primary disk - error", HealthStatus.Unhealthy)
        .AddFileWritePermissionsCheck(_environment.WebRootPath);

      services.AddHealthChecksUI().AddInMemoryStorage();
      services.AddControllersWithViews();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        // endpoints.MapHealthChecksWithJsonResponse("/api/health"); Custom JSON, no Health CHeck UI
        endpoints.MapHealthChecks("/api/health", new HealthCheckOptions
        {
          // Predicate = x => !x.Tags.Any(t => t == "Paid"), // Filter by tag
          // ResultStatusCodes = new Dictionary<HealthStatus, int> { { HealthStatus.Degraded, 500 }, { HealthStatus.Healthy, 200 }, { HealthStatus.Unhealthy, 503 } },
          ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        endpoints.MapHealthChecksUI();

        endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}
