namespace crossplatformapp.Utils.Startup;

public interface IStartup
{
    public void Register();
    public void Unregister();
    public StartupStatus Status { get; }
    static string PropertyName = "RunOnStartup";
}
public enum StartupStatus
{
    Unregistered,
    Registered,
    DisabledByUser
}