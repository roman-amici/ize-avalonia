using CommunityToolkit.Mvvm.ComponentModel;
using Ize.Services;

namespace Ize.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(ServicesCollection servicesCollection)
    {
        MainMenuViewModel = new MainMenuViewModel(servicesCollection.RecentFileService);
    }

    [ObservableProperty] private MainMenuViewModel mainMenuViewModel;
}
