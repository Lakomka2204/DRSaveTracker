using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DRSTCore
{
    public partial class SaveFileInfo : ObservableObject
    {
        [GeneratedRegex(@"filech(\d)_([012])")]
        private static partial Regex FileNameRegex();
        [ObservableProperty]
        private int chapter = 0;
        [ObservableProperty]
        private int slot = -1;
        [ObservableProperty]
        private string name = "EMPTY";
        [ObservableProperty]
        private TimeSpan playTime = TimeSpan.Zero;
        [ObservableProperty]
        private int room = 0;
        [ObservableProperty]
        private string? roomName = "N/A";
        [ObservableProperty]
        private string fileName = string.Empty;
        [ObservableProperty]
        private string originalFileName = string.Empty;
        [ObservableProperty]
        private DateTime lastWrite = DateTime.UnixEpoch;
        [ObservableProperty]
        private bool fileExists = false;
        [ObservableProperty]
        private SaveFileInfo[] backups = [];
        public RoomMapper? Mapper { get; set; }
        public SaveFileInfo(string filename)
        {
            FileName = filename;
            var name = Path.GetFileName(filename);
            var match = FileNameRegex().Match(name);
            if (!match.Success)
                return;
                // throw new ArgumentException("Filename regex unsuccessful", nameof(filename));
            OriginalFileName = match.Groups[0].Value;
            Chapter = int.Parse(match.Groups[1].Value);
            var slot = int.Parse(match.Groups[2].Value) + 1;
            Slot = slot;
            FileExists = File.Exists(FileName);
            if (!FileExists)
                return;
            FileInfo fi = new(filename);
            LastWrite = fi.LastWriteTime;
            var lines = File.ReadLines(filename);
            Name = lines.First() ?? "EMPTY";
            var rs = lines.TakeLast(2).ToArray();
            var room = rs[0];
            Room = int.Parse(room);
            var steps = rs[1];
            var secs = Math.Floor(double.Parse(steps.Trim(), CultureInfo.InvariantCulture) / 30);
            PlayTime = TimeSpan.FromSeconds(secs);
        }
        public SaveFileInfo(string filename, RoomMapper? roomMapper) : this(filename)
        {
            if (roomMapper != null)
                RoomName = roomMapper[Room];
            Mapper = roomMapper;
        }
        public SaveFileInfo(string filename, RoomMapper? roomMapper, string? backupFolder)
            : this(filename,roomMapper)
        {
            if (backupFolder == null)
                return;
            GetBackups(backupFolder);
        }
        public void GetBackups(string backupFolder)
        {
            var concreteBackupFolder = Path.Join(backupFolder, OriginalFileName);
            if (!Directory.Exists(concreteBackupFolder)) return;
            // сделать так чтобы первый отображался как последний (latest)
            Backups = [.. Directory
                .GetFiles(concreteBackupFolder,"filech*")
                .Select(f => new SaveFileInfo(f, Mapper))
                .OrderByDescending(f => f.LastWrite)];
        }
        public void MakeBackup(string backupFolder)
        {
            if (FileExists)
            {
                var backupConcreteFolder = Path.Join(backupFolder, OriginalFileName);
                if (!Directory.Exists(backupConcreteFolder))
                    Directory.CreateDirectory(backupConcreteFolder);
                var backupName = string.Format("{0}_{1}",OriginalFileName, LastWrite.ToFileTimeUtc());
                var backupFileName = Path.Join(backupConcreteFolder, backupName);
                File.Copy(FileName, backupFileName,true);
            }
            GetBackups(backupFolder);
        }
        public string RestoreToOriginal(string originalFolder)
        {
            var originalFilename = Path.Join(originalFolder, OriginalFileName);
            File.Copy(FileName, originalFilename, true);
            File.Delete(FileName);
            return originalFilename;
        }
        
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendFormat("Chapter {0}\t", Chapter);
            sb.AppendFormat("Slot {0}\t", Slot.ToString());
            sb.AppendFormat("{0} - {1}\t", Name.PadRight(12),PlayTime);
            if (RoomName != null)
                sb.AppendFormat("- {0}({1})", RoomName,Room);
            else
                sb.AppendFormat("- Room id: {0}",Room);
            return sb.ToString();
        }
    }
public enum Slot
{
    None = -1,
    First = 0,
    Second = 1,
    Third = 2,

}

}
