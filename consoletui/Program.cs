using System.Diagnostics;
using DRSTCore;
static int Exit(string s)
{
    Console.WriteLine(s);
    return 1;
}
Stopwatch sw = Stopwatch.StartNew();
Console.WriteLine("Start {0}", sw.Elapsed);
var bfolder = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DELTARUNE");
var files = Directory.GetFiles(bfolder,"filech*");
Console.WriteLine("Preloading room info");
var rm = new RoomMapper();
await rm.LoadRoomInfo(null);
Console.WriteLine("Finished {0}",sw.Elapsed);
List<SaveFileInfo> sfInfo = [];
foreach(var file in files)
{
    Console.WriteLine(Path.GetFileName(file));
    var sf = new SaveFileInfo(file,rm);
    Console.WriteLine(sf);
    sfInfo.Add(sf);
    Console.WriteLine("Elapsed time: {0}", sw.Elapsed);
}
sw.Stop();
Console.WriteLine("Start watch");
FileSystemWatcher fsw = new(bfolder, "filech*")
{
};
fsw.Changed += Fsw_Changed;
fsw.Created += Fsw_Changed;
fsw.Deleted += Fsw_Changed;

void Fsw_Changed(object sender, FileSystemEventArgs e)
{
    var f = (sender as FileSystemWatcher)!;
    try
    {
    f.EnableRaisingEvents = false;
    Console.WriteLine("Changed {0} ({1})", e.Name, e.ChangeType.ToString());

    }
    finally
    {
        f.EnableRaisingEvents = true;
    }
    
}
fsw.EnableRaisingEvents = true;
ConsoleKey k = ConsoleKey.None;
do
{
    var cki = Console.ReadKey(true);
    k = cki.Key;
}
while (k != ConsoleKey.Q);
Exit("lol");