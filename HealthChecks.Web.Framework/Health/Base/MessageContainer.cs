using Newtonsoft.Json;

namespace HealthChecks.Web.Framework.Health
{
  /// <summary>
  /// Container class to hold a text message and an additional info object. Used by health check providers to expose additional information.
  /// </summary>
  public class MessageContainer
  {
    /// <summary>
    /// Creates a new instance of the MessageContainer class.
    /// </summary>
    /// <param name="message">The info message to add.</param>
    public MessageContainer(string message)
    {
      Message = message;
    }

    /// <summary>
    /// Creates a new instance of the MessageContainer class.
    /// </summary>
    /// <param name="message">The info message to add.</param>
    /// <param name="additionalInfo">An object with additional info for the health check provider point.</param>
    public MessageContainer(string message, object additionalInfo)
    {
      Message = message;
      AdditionalInfo = additionalInfo;
    }

    /// <summary>
    /// Gets or sets the info message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets an object with additional info for the health check provider point.
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] // See https://www.newtonsoft.com/json/help/html/JsonPropertyPropertyLevelSetting.htm
    public object AdditionalInfo { get; set; }
  }
}