using CommunityToolkit.Mvvm.ComponentModel;

namespace Ize.ViewModels;

public partial class RecentFileModel : ObservableObject
{

    [ObservableProperty] string fileName = string.Empty;
    [ObservableProperty] string fullPath = string.Empty;
    [ObservableProperty] RecentFileType recentFileType;
}

public enum RecentFileType
{
    Deck,
    Piles,
    Other,
}