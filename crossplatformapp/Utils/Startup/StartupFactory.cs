
using System;

namespace crossplatformapp.Utils.Startup;
public static class StartupFactory
{
    public static IStartup Create()
    {
        if (OperatingSystem.IsWindows())
            return new WindowsStartup();

        throw new NotSupportedException("your os is not supported lol get rekt");
    }
}