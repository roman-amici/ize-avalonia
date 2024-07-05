using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Ize.Services;

namespace Ize.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{

    private readonly MainMenuViewModel mainMenuViewModel;
    private readonly PracticeRunViewModel practiceRunViewModel;

    [ObservableProperty] private ObservableObject? selectedViewModel;

    public MainWindowViewModel(ServicesCollection servicesCollection)
    {
        mainMenuViewModel = new MainMenuViewModel(servicesCollection.RecentFileService, servicesCollection.NavigationService);
        practiceRunViewModel = new PracticeRunViewModel();

        servicesCollection.NavigationService.NavigationRequested += Navigate;
    }

    private void Navigate(MainWindowView view, object? sessionObject)
    {
        switch (view)
        {
            case MainWindowView.MainMenu:
                SelectedViewModel = mainMenuViewModel;
                break;
            case MainWindowView.PracticeRun:
                if (sessionObject is PracticeRunService practiceRun)
                {
                    practiceRunViewModel.ActivateSession(practiceRun);
                    SelectedViewModel = practiceRunViewModel;
                }
                else
                {
                    throw new InvalidOperationException("Invalid practice run");
                }

                break;
        }
    }
}
