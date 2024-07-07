using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ize.Model;

public partial class RecentFileModel : ObservableObject
{
    public RecentFileModel(string filePath)
    {
        FullPath = Path.GetFullPath(filePath);

        var fileType = RecentFileType.Other;
        if (Path.GetExtension(fullPath) == ".deck") {
            fileType = RecentFileType.Deck;
        } else if (Path.GetExtension(fullPath) == ".piles") {
            fileType = RecentFileType.Piles;
        }

        RecentFileType = fileType;
        FileName = Path.GetFileName(fullPath);
    }

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