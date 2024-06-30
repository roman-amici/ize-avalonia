using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ize.ViewModels;

public partial class MainMenuViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<RecentFileModel> recentFiles;

}