namespace DRSaveTracker
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            bool silent = args.Contains("-s");
            var main = new NewMain()
            {
                Visible = !silent,
            };
            Application.Run(main);
        }
        public static string AppName = "DRSaveTracker";
    }
}