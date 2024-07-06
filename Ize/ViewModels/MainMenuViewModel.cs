using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ize.Library;
using Ize.Model;
using Ize.Services;

namespace Ize.ViewModels;

public partial class MainMenuViewModel : ObservableObject
{
    private readonly RecentFileService recentFileService;
    private readonly NavigationService navigationService;

    public Func< string, IEnumerable<FilePickerFileType>, Task<string?>>? PickFile {get; set;}

    public MainMenuViewModel(RecentFileService recentFileService, NavigationService navigationService)
    {
        this.recentFileService = recentFileService;
        this.navigationService = navigationService;

        foreach(var filePath in recentFileService!.FilePaths)
        {
            recentFiles.Add(new RecentFileModel(filePath));
        }        
    }

    [ObservableProperty] private ObservableCollection<RecentFileModel> recentFiles = new();

    [RelayCommand]
    private async Task ResumePractice()
    {
        var task = PickFile?.Invoke("Select Practice Run", [new FilePickerFileType("piles"){
            Patterns = ["*.piles"]
        }]);

        if (task == null)
        {
            return;
        }

        var filePath = await task;

        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }

        RecentFiles.Add(new RecentFileModel(filePath));
        recentFileService.FilePaths.Add(filePath);

        var pilesService = await PracticeRunService.CreateFromPiles(filePath);
        navigationService.NavigateMain(MainWindowView.PracticeRun, pilesService);

    }


    [RelayCommand]
    private void NewPractice()
    {
        
    }


    [RelayCommand]
    private void EditDeck()
    {
        
    }


    [RelayCommand]
    private void NewDeck()
    {
        
    }

    public event EventHandler<IzeDeck>? DeckSelected;
    public event EventHandler<PracticeRunService>? PracticeRunSelected;

}