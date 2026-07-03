using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using File = System.IO.File;
namespace DRSaveTracker
{
    internal static class StartupManager
    {
        private static string ShortcutPath =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                $"{Application.ProductName}.lnk");

        public static bool IsEnabled()
        {
            return File.Exists(ShortcutPath);
        }

        public static void SetEnabled(bool enabled)
        {
            if (enabled)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ShortcutPath)!);
                Type shellType = Type.GetTypeFromProgID("WScript.Shell")!;
                dynamic shell = Activator.CreateInstance(shellType)!;
                dynamic shortcut = shell.CreateShortcut(ShortcutPath);

                string exe = Application.ExecutablePath;

                shortcut.TargetPath = exe;
                shortcut.Arguments = "-s";
                shortcut.WorkingDirectory = Path.GetDirectoryName(exe);
                shortcut.Description = Application.ProductName;
                shortcut.IconLocation = exe;
                shortcut.Save();
            }
            else
            {
                if (File.Exists(ShortcutPath))
                    File.Delete(ShortcutPath);
            }
        }
    }
}
