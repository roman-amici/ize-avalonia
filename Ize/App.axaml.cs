using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Ize.Services;
using Ize.ViewModels;
using Ize.Views;

namespace Ize;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var servicesCollection = new ServicesCollection();
            var vm = new MainWindowViewModel(servicesCollection);
            servicesCollection.NavigationService.NavigateMain(MainWindowView.MainMenu, null);

        
            desktop.MainWindow = new MainWindow
            {
                DataContext = vm,
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}