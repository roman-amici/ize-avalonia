using System;

namespace Ize.Services;

public enum MainWindowView
{
    MainMenu,
    PracticeRun,
    DeckEditor
}

public class NavigationService
{
    public void NavigateMain(MainWindowView view, object? sessionData)
    {
        NavigationRequested?.Invoke(view, sessionData);
    }

    public Action<MainWindowView, object?>? NavigationRequested {get; set;}
}