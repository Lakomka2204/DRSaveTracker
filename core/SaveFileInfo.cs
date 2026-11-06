using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
namespace DRSTCore
{
    public partial class SaveFileInfo : ObservableObject
    {
        [GeneratedRegex(@"^(filech(\d)_([012]))(?:_(\d+))?$")]
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
        [ObservableProperty]
        private long backupSize = 0;
        [ObservableProperty]
        private string iniSection = string.Empty;
        public RoomMapper? Mapper { get; set; }
        public FileInfo FileInfo { get; set; }
        public byte[] Hash {get;set;} = [];
        public int Level { get; set; } = 0;
        public static string GetIniSection(int chapter, int slot)
        {
            if (chapter >= 2)
                return string.Format("G_{0}_{1}",chapter,slot);
            else
                return string.Format("G{0}",slot);
        }
        public SaveFileInfo(string filename)
        {
            FileName = filename;
            FileInfo = new(FileName);
            var name = Path.GetFileName(FileName);
            var match = FileNameRegex().Match(name);
            if (!match.Success)
                return;
            // throw new ArgumentException("Filename regex unsuccessful", nameof(filename));
            OriginalFileName = match.Groups[1].Value;
            Chapter = int.Parse(match.Groups[2].Value);
            var slot = int.Parse(match.Groups[3].Value) + 1;
            Slot = slot;
            IniSection = GetIniSection(Chapter,Slot - 1);
            FileExists = FileInfo.Exists;
            if (!FileExists)
                return;
            using var stream = FileInfo.OpenRead();
            Hash = MD5.HashData(stream);
            stream.Seek(0, SeekOrigin.Begin);
            LastWrite = FileInfo.LastWriteTime;
            using StreamReader sr = new(stream);
            var lines = sr.ReadLines();
            Name = lines.FirstOrDefault() ?? "[EMPTY]";
            var lv = lines.Take(12).Last().Trim();
            Level = int.Parse(lv);
            var rs = lines.TakeLast(2).ToArray();
            var room = rs[0];
            Room = int.Parse(room);
            var steps = rs[1];
            var secs = Math.Floor(double.Parse(steps.Trim(), CultureInfo.InvariantCulture) / 30);
            PlayTime = TimeSpan.FromSeconds(secs);
        }
        public SaveFileInfo(string filename, RoomMapper? roomMapper) : this(filename)
        {
            Mapper = roomMapper;

            if (roomMapper is null)
                return;
            var rName = roomMapper[Room];
            if (rName is null)
                Console.WriteLine("Unknown room id or ch2 Dark World? (20085): {0}, caller: {1}",Room,FileName);
            RoomName = rName ?? "Dark World?";
        }
        public SaveFileInfo(string filename, RoomMapper? roomMapper, string? backupFolder)
            : this(filename, roomMapper)
        {
            if (backupFolder == null)
                return;
            GetBackups(backupFolder);
        }
        public void GetBackups(string backupFolder)
        {
            var concreteBackupFolder = Path.Join(backupFolder, OriginalFileName);
            if (!Directory.Exists(concreteBackupFolder)) return;
            Backups = [.. Directory
                .GetFiles(concreteBackupFolder,SaveFileWatcher.FilePattern)
                .Select(f => new SaveFileInfo(f, Mapper))
                .OrderByDescending(f => f.LastWrite == LastWrite)
                .ThenByDescending(f => f.LastWrite)];
            BackupSize = Backups.Sum(f => f.FileInfo.Length);
        }
        public void MakeBackup(string backupFolder)
        {
            if (FileExists)
            {
                var backupConcreteFolder = Path.Join(backupFolder, OriginalFileName);
                if (!Directory.Exists(backupConcreteFolder))
                    Directory.CreateDirectory(backupConcreteFolder);
                var backupName = string.Format("{0}_{1}", OriginalFileName, LastWrite.ToFileTimeUtc());
                var backupFileName = Path.Join(backupConcreteFolder, backupName);
                File.Copy(FileName, backupFileName, true);
            }
            GetBackups(backupFolder);
        }
        public string RestoreToOriginal(string originalFolder)
        {
            System.Console.WriteLine("Restoring original {0} to original folder specified {1}",OriginalFileName,originalFolder);
            var originalFilename = Path.Join(originalFolder, OriginalFileName);
            File.Copy(FileName, originalFilename, true);
            File.Delete(FileName);
            UpdateIniFile(originalFolder);
            return originalFilename;
        }
        private void UpdateIniFile(string originalFolder)
        {
            System.Console.WriteLine("Changing INI file section {0}",IniSection);
            var iniFilename = Path.Join(originalFolder,SaveFileWatcher.IniFilename);
            using StreamReader sr = new(iniFilename);
            var lines = sr.ReadLines();
            IniParser iniData = new(lines);
            sr.Close();
            var s = iniData.GetSection(IniSection,true);
            // if (s is null)
            // {
            //     Console.WriteLine("Couldn't find {0} section in INI",IniSection);
            //     return;
            // }
            var room = s.GetParameter("Room",true);
            room.Value = IniParameter.ConvertNumbersToString(Room);
            var date = s.GetParameter("Date",true);
            date.Value = IniParameter.ConvertNumbersToString(LastWrite.ToOADate());
            var time = s.GetParameter("Time",true);
            time.Value = IniParameter.ConvertNumbersToString((int)(PlayTime.TotalSeconds*30));
            var name = s.GetParameter("Name",true);
            name.Value = Name;
            var level = s.GetParameter("Level",true);
            level.Value = IniParameter.ConvertNumbersToString(Level);
            using StreamWriter sw = new(iniFilename);
            sw.Write(iniData);
            Console.WriteLine("Updated INI File");
        }
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendFormat("Chapter {0}\t", Chapter);
            sb.AppendFormat("Slot {0}\t", Slot.ToString());
            sb.AppendFormat("{0} - {1}\t", Name.PadRight(12), PlayTime);
            if (RoomName != null)
                sb.AppendFormat("- {0}({1})", RoomName, Room);
            else
                sb.AppendFormat("- Room id: {0}", Room);
            return sb.ToString();
        }
    }
}
