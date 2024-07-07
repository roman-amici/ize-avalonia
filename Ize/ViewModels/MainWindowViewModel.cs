using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Ize.Services;

namespace Ize.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{

    public MainMenuViewModel MainMenuViewModel {get;}
    public PracticeRunViewModel PracticeRunViewModel {get;}

    [ObservableProperty] private ObservableObject? selectedViewModel;

    public MainWindowViewModel(ServicesCollection servicesCollection)
    {
        MainMenuViewModel = new MainMenuViewModel(servicesCollection.RecentFileService, servicesCollection.NavigationService);
        PracticeRunViewModel = new PracticeRunViewModel(servicesCollection.NavigationService);

        servicesCollection.NavigationService.NavigationRequested += Navigate;
    }

    private void Navigate(MainWindowView view, object? sessionObject)
    {
        switch (view)
        {
            case MainWindowView.MainMenu:
                SelectedViewModel = MainMenuViewModel;
                break;
            case MainWindowView.PracticeRun:
                if (sessionObject is PracticeRunService practiceRun)
                {
                    PracticeRunViewModel.ActivateSession(practiceRun);
                    SelectedViewModel = PracticeRunViewModel;
                }
                else
                {
                    throw new InvalidOperationException("Invalid practice run");
                }

                break;
        }
    }
}
