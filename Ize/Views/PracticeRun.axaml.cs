using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
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

    private async Task<string?> GetSavePiles(string deckName)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null)
        {
            return null;
        }

        var suggestedName = string.IsNullOrEmpty(deckName) ? "practice" : deckName;
        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save Piles",
            FileTypeChoices = [new FilePickerFileType("Piles"){
                Patterns = [".piles"]
            }],
            DefaultExtension = ".piles",
            SuggestedFileName = suggestedName
        });

        if (file == null)
        {
            return null;
        }

        var filePath = file.TryGetLocalPath();
        if (filePath == null)
        {
            return null;
        }

        return Path.GetFullPath(filePath);
    }
}