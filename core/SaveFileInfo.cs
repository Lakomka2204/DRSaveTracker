using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace DRSTCore
{
    public partial class SaveFileInfo
    {
        [GeneratedRegex(@"filech(\d)_(\d)(_b)?")]
        private static partial Regex FileNameRegex();
        public int Chapter { get; set; }
        public Slot Slot { get; set; }
        public SlotType Type { get; set; }
        public string Name { get; set; } = "EMPTY";
        public TimeSpan PlayTime { get; set; }
        public bool BSide { get; set; } = false;
        public int Room { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string? RoomName { get; set; }
        public DateTime LastWrite { get; set; } = DateTime.MinValue;
        public RoomMapper? Mapper { get; set; }
        public bool FileExists { get; set; }
        public SaveFileInfo(string filename)
        {
            FileName = filename;
            var name = Path.GetFileName(filename);
            var match = FileNameRegex().Match(name);
            if (!match.Success)
                throw new ArgumentException("Filename regex unsuccessful", nameof(filename));
            OriginalFileName = match.Groups[0].Value;
            Chapter = int.Parse(match.Groups[1].Value);
            var slot = int.Parse(match.Groups[2].Value);
            Slot = slot == 9 ? Slot.None : (Slot)(slot % 3);
            Type = (SlotType)(slot / 3);
            FileExists = File.Exists(FileName);
            if (!FileExists)
                return;
            if (match.Groups[3].Value.Length > 0)
                BSide = true;
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
        public SaveFileInfo[] GetBackups(string backupFolder)
        {
            var saveFolder = Path.Join(backupFolder, OriginalFileName);
            if (!Directory.Exists(saveFolder)) return [];
            return Directory
                .GetFiles(saveFolder,"filech*")
                .Select(f => new SaveFileInfo(f, Mapper))
                .OrderByDescending(f => f.LastWrite).ToArray();
        }
        public void MakeBackup(string backupFolder)
        {
            var backupConcreteFolder = Path.Join(backupFolder, OriginalFileName);
            if (!Directory.Exists(backupConcreteFolder))
                Directory.CreateDirectory(backupConcreteFolder);
            var backupName = string.Format("{0}_{1}",OriginalFileName, LastWrite.ToFileTimeUtc());
            var backupFileName = Path.Join(backupConcreteFolder, backupName);
            File.Copy(FileName, backupFileName,true);
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
            sb.AppendFormat("Chapter {0}{1}\t", Chapter,BSide ? "B" : string.Empty);
            sb.AppendFormat("Slot {0}({1})\t", Slot.ToString(),Type.ToString());
            sb.AppendFormat("- {0} - {1}\t", Name.PadRight(12),PlayTime);
            if (RoomName != null)
                sb.AppendFormat("- {0}({1})", RoomName,Room);
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
    public enum SlotType
    {
        Normal = 0,
        Completion = 1,
        Death = 3
    }
}
