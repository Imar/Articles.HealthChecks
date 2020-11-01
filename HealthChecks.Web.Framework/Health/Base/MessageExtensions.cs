using System.Collections.Generic;

namespace HealthChecks.Web.Framework.Health
{
  /// <summary>
  /// Custom methods for List of MessageContainer to expose easier Add methods.
  /// </summary>
  public static class MessageExtensions
  {
    /// <summary>
    /// Adds a new MessageContainer with just a message.
    /// </summary>
    /// <param name="list">The list to add to.</param>
    /// <param name="message">The message to add.</param>
    public static void Add(this List<MessageContainer> list, string message)
    {
      list.Add(new MessageContainer(message));
    }

    /// <summary>
    /// Adds a new MessageContainer with a message and an object with additional information.
    /// </summary>
    /// <param name="list">The list to add to.</param>
    /// <param name="message">The message to add.</param>
    /// <param name="additionalInfo">An object with additional info for the health check provider point.</param>
    public static void Add(this List<MessageContainer> list, string message, object additionalInfo)
    {
      list.Add(new MessageContainer(message, additionalInfo));
    }
  }
}