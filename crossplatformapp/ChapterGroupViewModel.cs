using System.Collections.ObjectModel;

public class ChapterGroupViewModel
{
    public string Header { get; }
    public ObservableCollection<SaveSlotViewModel> Items { get; } = new();

    public ChapterGroupViewModel(string header) => Header = header;
}