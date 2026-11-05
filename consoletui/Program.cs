
using System.Globalization;
using DRSTCore; 

System.Console.WriteLine(DateTime.FromOADate(46198.509578).ToOADate());
using StreamReader sr = new(@"C:\Users\Lakomka\AppData\Local\DELTARUNE\dr.ini");
var lines = sr.ReadLines();
IniParser pr = new(lines);

var s = pr.GetSection("G4");
if (s is not null)
{
    var p = s.GetParameter("Name");
    if (p is not null)
    {
        p.Value = "SEX";
    }
    var pdate = s.GetParameter("Date");
    System.Console.WriteLine("pdate: {0}",DateTime.FromOADate(double.Parse(pdate.Value,CultureInfo.InvariantCulture)));
    pdate.Value = IniParameter.ConvertNumbersToString(DateTime.Now.ToOADate());
    var ptime = s.GetParameter("Time");
    System.Console.WriteLine("timeee: {0}, MUST BE x2!!!",ptime.Value);
    var ptimeint = double.Parse(ptime.Value,CultureInfo.InvariantCulture)*2;
    ptime.Value = IniParameter.ConvertNumbersToString(ptimeint);
}
var pp = Path.GetTempFileName();
System.Console.WriteLine(string.Join('\n',pr.Sections.Select(c => c.Name)));
using StreamWriter sw = new(pp);
sw.Write(pr.ToString());
sw.Close();
Console.WriteLine(pp);
object[] testValues =
        {
            46198.509578,
            4523,
            4523.5f,
            383368L,
            -123.456789,
            0
        };

         foreach (var value in testValues)
        {
            try
            {
                Console.WriteLine(
                    $"{value} ({value.GetType().Name}) => {IniParameter.ConvertNumbersToString(value)}"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
        }
return;
string appName = "DRSaveTracker";

string saveFolder = Path.Join(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
            "DELTARUNE");
        string backupFolder = Path.Join(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
            appName,
            "Backups");
        string cacheFolder = Path.Join(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
            appName,
            "Cache");


Console.WriteLine("APP START");
Console.WriteLine("SAVE DIR {0}", saveFolder);
Console.WriteLine("BACKUP DIR {0}",backupFolder);
Console.WriteLine("CACHE DIR {0}",cacheFolder);

SaveFileWatcher sfw = new()
{
    BackupDirectory = backupFolder,
    WatchDirectory = saveFolder
};
Console.WriteLine("SFW DECLARED");
sfw.IsEnabled = true;
Console.WriteLine(string.Join("\n",sfw.Saves));
Console.WriteLine("SFW ENABLED");
Console.ReadLine();