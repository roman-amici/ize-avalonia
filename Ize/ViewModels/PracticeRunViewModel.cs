
using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ize.Library;
using Ize.Model;
using Ize.Services;

namespace Ize.ViewModels;

public partial class PracticeRunViewModel : ObservableObject
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

        for (var  i = 0 ; i < practiceRun.Piles.PilesOrder.Count; i++)
        {
            var pileName = practiceRun.Piles.PilesOrder[i];
            Piles.Add(new PileModel(){
                HotKey =  (char)('0' + (i + 1)),
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
            CurrentPileProgress++;
        }
        else
        {
            CurrentCard = null;
        }
    }

    [RelayCommand]
    public void MoveCurrentCard(string destinationPileName)
    {
        if (practiceRun == null)
        {
            return;
        }

        practiceRun.MoveTo(destinationPileName);

        var sourcePileModel = Piles.First(x => x.PileName == practiceRun.ActivePileName);
        sourcePileModel.RemainingCards =  practiceRun.Piles.Piles[practiceRun.ActivePileName].Count;

        var destinationPileModel = Piles.First( x => x.PileName == destinationPileName);
        destinationPileModel.RemainingCards = practiceRun.Piles.Piles[destinationPileName].Count;
    }


}