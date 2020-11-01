using System.Collections.Generic;
using Newtonsoft.Json;

namespace HealthChecks.Web.Framework.Health
{
  /// <summary>
  /// Provides health information of a resource.
  /// </summary>
  public class HealthCheckItemResult
  {
    /// <summary>
    /// Creates a new instance of the HealthCheck class.
    /// </summary>
    /// <param name="resourceName">The name of the resource that this health check belongs to.</param>
    /// <param name="sortOrder">The order to sort the results in.</param>
    /// <param name="friendlyName">The friendly name for the health check.</param>
    /// <param name="description">The description of the health check.</param>
    public HealthCheckItemResult(string resourceName, int sortOrder, string friendlyName, string description = null)
    {
      ResourceName = resourceName;
      SortOrder = sortOrder;
      FriendlyName = friendlyName;
      Description = description;
    }

    /// <summary>
    /// Gets the name of the resource.
    /// </summary>
    [JsonProperty(Order = -70)] // Push them to the top of the list, with the base properties coming first. https://stackoverflow.com/questions/32571695/order-of-fields-when-serializing-the-derived-class-in-json-net
    public string ResourceName { get; }

    /// <summary>
    /// Gets the friendly name for the health check .
    /// </summary>
    [JsonProperty(Order = -60)] // Push them to the top of the list, with the base properties coming first. https://stackoverflow.com/questions/32571695/order-of-fields-when-serializing-the-derived-class-in-json-net
    public string FriendlyName { get; }

    /// <summary>
    /// Gets or sets the description of the health check .
    /// </summary>
    [JsonProperty(Order = -50)] // Push them to the top of the list, with the base properties coming first. https://stackoverflow.com/questions/32571695/order-of-fields-when-serializing-the-derived-class-in-json-net
    public string Description { get; set; }

    /// <summary>
    /// Gets a value that indicates if the current resource is considered to be up and healthy, degraded, or unhealthy.
    /// </summary>
    [JsonIgnore]
    public virtual HealthState HealthState { get; set; }

    /// <summary>
    /// Gets a value that describes if the current resource is considered to be up and healthy, degraded, or unhealthy.
    /// </summary>
    [JsonProperty(Order = -40)] // Push them to the top of the list, with the base properties coming first. https://stackoverflow.com/questions/32571695/order-of-fields-when-serializing-the-derived-class-in-json-net
    public virtual string State => HealthState.ToString();

    /// <summary>
    /// Gets a collection of additional messages.
    /// </summary>
    [JsonProperty(Order = 1000)] // To the bottom of the list
    public List<MessageContainer> Messages { get; } = new List<MessageContainer>();

    /// <summary>
    /// Defines the order of this provider in the results.
    /// </summary>
    [JsonIgnore]
    public int SortOrder { get; }
  }
}