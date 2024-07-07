using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Ize.ViewModels;

namespace Ize.Views;

public partial class PracticeRun : UserControl
{
    public PracticeRun()
    {
        InitializeComponent();

        PropertyChanged += AddViewModelEvents;

    }

    private void AddViewModelEvents(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == DataContextProperty)
        {
            if (DataContext is PracticeRunViewModel vm)
            {
                vm.GetSavePilesPath = GetSavePiles;
            }
        }

    }

    private async Task<string?> GetSavePiles()
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null)
        {
            return null;
        }

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions{
            AllowMultiple = false,
            Title = "Save Piles",
            FileTypeFilter = [new FilePickerFileType("Piles"){
                Patterns = [".piles"]
            }]
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