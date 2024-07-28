using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Ize.ViewModels;

namespace Ize.Views;

public partial class DeckEditor : UserControl
{
    public DeckEditor()
    {
        InitializeComponent();
        PropertyChanged += AddViewModelEvents;

    }

    private void AddViewModelEvents(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == DataContextProperty)
        {
            if (DataContext is DeckEditorViewModel vm)
            {
                vm.GetSavePath = GetSaveDeckPath;
            }
        }
    }

    private async Task<string?> GetSaveDeckPath()
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null)
        {
            return null;
        }

        var suggestedName = "cards";
        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save Deck",
            FileTypeChoices = [new FilePickerFileType("Deck"){
                Patterns = [".deck"]
            }],
            DefaultExtension = ".deck",
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