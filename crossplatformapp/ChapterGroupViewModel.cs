using System.Collections.ObjectModel;
namespace crossplatformapp;

public class ChapterGroupViewModel
{
    public string Header { get; }
    public ObservableCollection<SaveSlotViewModel> Items { get; } = new();

    public ChapterGroupViewModel(string header) => Header = header;
}