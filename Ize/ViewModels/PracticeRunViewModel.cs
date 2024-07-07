
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ize.Library;
using Ize.Model;
using Ize.Services;

namespace Ize.ViewModels;

public partial class PracticeRunViewModel(NavigationService navigationService) : ObservableObject
{
    private PracticeRunService? practiceRun;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasCurrentCard))]
    private CardModel? currentCard;
    [ObservableProperty] private string deckName = string.Empty;
    [ObservableProperty] private ObservableCollection<PileModel> piles = new();

    [ObservableProperty] private int currentPileTotal;
    [ObservableProperty] private int currentPileProgress;

    public bool HasCurrentCard => CurrentCard != null;


    public void ActivateSession(PracticeRunService practiceRun)
    {
        this.practiceRun = practiceRun;
        Piles.Clear();

        for (var i = 0; i < practiceRun.Piles.PilesOrder.Count; i++)
        {
            var pileName = practiceRun.Piles.PilesOrder[i];
            var keyNumber = i + 1;
            Piles.Add(new PileModel()
            {
                HotKey = PileModel.NumberKey(keyNumber),
                DisplayHotKey = keyNumber.ToString(),
                PileName = pileName,
                RemainingCards = practiceRun.Piles.Piles[pileName].Count
            });
        }

        DeckName = practiceRun.Deck.Name ?? string.Empty;
        CurrentPileTotal = practiceRun.GetActivePile().Count;
        NextCard();

    }

    public void NextCard()
    {
        if (practiceRun == null)
        {
            return;
        }

        var nextCard = practiceRun.CurrentCard();

        if (nextCard != null)
        {
            CurrentCard = new CardModel(nextCard);
            CurrentPileProgress = CurrentPileTotal - practiceRun.GetActivePile().Count + 1;
        }
        else
        {
            CurrentCard = null;
        }
    }

    [RelayCommand]
    public void MoveToPile(string destinationPileName)
    {
        if (practiceRun == null)
        {
            return;
        }

        if (destinationPileName == practiceRun.ActivePileName)
        {
            practiceRun.Skip();
        }
        else
        {
            practiceRun.MoveTo(destinationPileName);

            var sourcePileModel = Piles.First(x => x.PileName == practiceRun.ActivePileName);
            sourcePileModel.RemainingCards = practiceRun.Piles.Piles[practiceRun.ActivePileName].Count;

            var destinationPileModel = Piles.First(x => x.PileName == destinationPileName);
            destinationPileModel.RemainingCards = practiceRun.Piles.Piles[destinationPileName].Count;
        }

        NextCard();
    }

    [RelayCommand]
    public void Reshuffle(string destinationPileName)
    {
        if (practiceRun == null)
        {
            return;
        }

        practiceRun.MoveToActive(destinationPileName);
        practiceRun.Shuffle();

        foreach (var pile in Piles)
        {
            pile.RemainingCards = practiceRun.Piles.Piles[pile.PileName].Count;
        }

        CurrentPileTotal = practiceRun.GetActivePile().Count;

        NextCard();
    }

    [RelayCommand]
    public void Flip()
    {
        CurrentCard?.Flip();
    }

    [RelayCommand]
    public async Task Finish()
    {
        if (practiceRun != null)
        {
            if (string.IsNullOrEmpty(practiceRun.Piles.OriginalFilePath))
            {
                if (GetSavePilesPath != null)
                {
                    practiceRun.Piles.OriginalFilePath = await GetSavePilesPath() ?? string.Empty;
                }
            }

            if (!string.IsNullOrEmpty(practiceRun.Piles.OriginalFilePath))
            {
                await practiceRun.Piles.SaveToFile();
            }
        }

        practiceRun = null;
        navigationService.NavigateMain(MainWindowView.MainMenu, null);
    }

    [RelayCommand]
    public void HotKey(string displayHotKey)
    {
        var pile = Piles.FirstOrDefault(x => x.DisplayHotKey == displayHotKey);
        if (pile != null)
        {
            MoveToPile(pile.PileName);
        }
    }

    public Func<Task<string?>>? GetSavePilesPath { get; set; }
}