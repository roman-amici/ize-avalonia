
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Ize.Services;

namespace Ize.ViewModels;

public class PracticeRunViewModel : ObservableObject
{
    private PracticeRunService? practiceRun;

    public void ActivateSession(PracticeRunService practiceRun)
    {
        this.practiceRun = practiceRun;
    }
}