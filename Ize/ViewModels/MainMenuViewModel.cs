using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ize.Services;

namespace Ize.ViewModels;

public partial class MainMenuViewModel : ObservableObject
{
    private RecentFileService recentFileService;

    public MainMenuViewModel(RecentFileService recentFileService)
    {
        this.recentFileService = recentFileService;

        foreach(var filePath in recentFileService!.FilePaths)
        {
            var fileType = RecentFileType.Other;
            if (Path.GetExtension(filePath) == ".deck") {
                fileType = RecentFileType.Deck;
            } else if (Path.GetExtension(filePath) == ".pile") {
                fileType = RecentFileType.Piles;
            }

            recentFiles.Add(new RecentFileModel(){
                FileName = Path.GetFileName(filePath),
                FullPath = Path.GetFullPath(filePath),
                RecentFileType = fileType
            });
        }

        
    }

    [ObservableProperty] private ObservableCollection<RecentFileModel> recentFiles = new();

    [RelayCommand]
    private void ResumePractice()
    {
        
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

}