namespace HealthChecks.Web.Core.Mail
{
public class SmtpSettings
{
  public string MailServer { get; private set; }
  public int Port { get; private set; }
  public string UserName { get; private set; }
  public string Password { get; private set; }
}
}