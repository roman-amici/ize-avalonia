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

        var servicesCollection = new ServicesCollection();
        DataContext = new MainWindowViewModel(servicesCollection);
        
    }
}