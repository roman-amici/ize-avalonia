using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Ize.Services;
using Ize.ViewModels;

namespace Ize.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();        
    }

    public void Window_Closing(object? sender, WindowClosingEventArgs e )
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.MainMenuViewModel.SaveRecentFiles();
        }
    }
}