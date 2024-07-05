using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Ize.Services;
using Ize.ViewModels;

namespace Ize.Views;

public partial class MainMenu : UserControl
{
    public MainMenu()
    {
        InitializeComponent();

        PropertyChanged += AddViewModelEvents;


    }

    private void AddViewModelEvents(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == DataContextProperty)
        {
            if (DataContext is MainMenuViewModel vm)
            {
                vm.PickFile = PickFile;
            }
        }

    }

    private async Task<string?> PickFile(string title, IEnumerable<FilePickerFileType> fileTypes)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null)
        {
            return null;
        }

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions{
            AllowMultiple = false,
            Title = "Pick",
            FileTypeFilter = fileTypes.ToArray()
        });

        if (files.Count == 0){
            return null;
        }

        var filePath = files[0].TryGetLocalPath();
        if (filePath == null)
        {
            return null;
        }

        return Path.GetFullPath(filePath);
    }
}