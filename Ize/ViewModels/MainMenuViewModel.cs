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

    public Func<string, IEnumerable<FilePickerFileType>, Task<string?>>? PickFile { get; set; }

    public MainMenuViewModel(RecentFileService recentFileService, NavigationService navigationService)
    {
        this.recentFileService = recentFileService;
        this.navigationService = navigationService;

        foreach (var filePath in recentFileService!.FilePaths)
        {
            recentFiles.Add(new RecentFileModel(filePath));
        }
    }

    [ObservableProperty] 
    private ObservableCollection<RecentFileModel> recentFiles = new();

    public void SaveRecentFiles()
    {
        recentFileService.SaveToFile();
    }

    public void AddToRecentFiles(string filePath)
    {
        if (RecentFiles.FirstOrDefault(x => x.FullPath == filePath) == null)
        {
            recentFileService.FilePaths.Add(filePath);
            RecentFiles.Add(new RecentFileModel(filePath));
        }
    }

    private async Task<string?> PickDeck()
    {
        var task = PickFile?.Invoke("Select Deck", [new FilePickerFileType("deck"){
            Patterns = ["*.deck"]
        }]);

        if (task == null)
        {
            return null;
        }

        var filePath = await task;


        if (string.IsNullOrEmpty(filePath))
        {
            return null;
        }

        AddToRecentFiles(filePath);

        return filePath;
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

        var piles = await IzePiles.LoadFromFile(filePath);
        var pilesDirectory = Path.GetDirectoryName(filePath) ?? "./";

        var deckPath = Path.Combine(pilesDirectory, piles.DeckPath);
        if (!File.Exists(deckPath))
        {
            deckPath = await PickDeck();
        }

        if (string.IsNullOrEmpty(deckPath))
        {
            return;
        }

        var deck = await IzeDeck.LoadFromFile(deckPath);

        var pilesService = new PracticeRunService(piles.PilesOrder.First(), deck, piles);
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
    private async Task NewPractice(string? filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            filePath = await PickDeck();
        }

        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }

        var deck = await IzeDeck.LoadFromFile(filePath);

        var pilesService = PracticeRunService.CreateFromDeck(deck);
        navigationService.NavigateMain(MainWindowView.PracticeRun, pilesService);
    }


    [RelayCommand]
    private async Task EditDeck(string? filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            filePath = await PickDeck();

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }
        }

        var deck = await IzeDeck.LoadFromFile(filePath);

        navigationService.NavigateMain(MainWindowView.DeckEditor, deck);
    }


    [RelayCommand]
    private void NewDeck()
    {
        navigationService.NavigateMain(MainWindowView.DeckEditor, null);
    }
}