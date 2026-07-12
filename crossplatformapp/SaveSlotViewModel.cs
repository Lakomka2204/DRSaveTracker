using DRSTCore;

public class SaveSlotViewModel
{
    public SaveFileInfo Info { get; }

    public SaveSlotViewModel(SaveFileInfo info) => Info = info;

    public string FileName => Info.FileName;      // Tag/ToolTip equivalent
    public string ToolTip => Info.FileName;

    public string Slot => Info.Slot.ToString();
    public string Name => Info.Name;
    public string PlayTime => Info.PlayTime.ToString();
    public string Room => Info.RoomName ?? Info.Room.ToString();
    public string LastWrite => Info.LastWrite.ToString();
}