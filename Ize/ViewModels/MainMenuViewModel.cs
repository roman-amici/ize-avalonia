using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

    public void SaveRecentFiles()
    {
        recentFileService.SaveToFile();
    }

    private void AddToRecentFiles(string filePath)
    {
        if (RecentFiles.FirstOrDefault(x => x.FullPath == filePath) == null)
        {
            recentFileService.FilePaths.Add(filePath);
            RecentFiles.Add(new RecentFileModel(filePath));
        }
    }

    [RelayCommand]
    private async Task ResumePractice(string? filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            var task = PickFile?.Invoke("Select Practice Run", [new FilePickerFileType("piles"){
                Patterns = ["*.piles"]
            }]);

            if (task == null)
            {
                return;
            }

            filePath = await task;
        }

        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }

        AddToRecentFiles(filePath);

        var pilesService = await PracticeRunService.CreateFromPiles(filePath);
        navigationService.NavigateMain(MainWindowView.PracticeRun, pilesService);
    }

    [RelayCommand]
    private async Task RecentSelected(RecentFileModel file)
    {
        if (file.RecentFileType == RecentFileType.Piles)
        {
            await ResumePractice(file.FullPath);
        }
        else if (file.RecentFileType == RecentFileType.Deck)
        {
            await EditDeck(file.FullPath);
        }

        // TODO: Infer type from file for "other".
    }


    [RelayCommand]
    private void NewPractice()
    {
        
    }


    [RelayCommand]
    private async Task EditDeck(string filePath)
    {
        
    }


    [RelayCommand]
    private void NewDeck()
    {
        
    }

    public event EventHandler<IzeDeck>? DeckSelected;
    public event EventHandler<PracticeRunService>? PracticeRunSelected;

}