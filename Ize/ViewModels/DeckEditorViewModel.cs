using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ize.Library;
using Ize.Model;
using Ize.Services;

namespace Ize.ViewModels;

public partial class DeckEditorViewModel(NavigationService navigation) : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<CardModel> cards = new();

    [ObservableProperty]
    private CardModel? selectedCard;

    public IzeDeck? Deck {get; set;}

    public void Activate(IzeDeck deck, ulong? selectedCard)
    {
        Deck = deck;

        Cards.Clear();
        foreach(var card in Deck.Cards.Values)
        {
            var cardModel = new CardModel(card);
            Cards.Add(cardModel);

            if (selectedCard == card.CardIndex)
            {
                SelectedCard = cardModel;
            }
        }


        if (SelectedCard == null)
        {
            if (Cards.Count == 0)
            {
                NewCard();
            }

            SelectedCard = Cards.FirstOrDefault();
        }
    }

    [RelayCommand]
    public void NewCard()
    {
        ulong nextIndex = 1;
        if(Cards.Count > 0)
        {
            nextIndex = Cards.Max(x => x.CardIndex) + 1;
        }
        Cards.Add(new CardModel(){
            CardIndex = nextIndex
        });

        SelectedCard = Cards.Last();
    }

    [RelayCommand]
    public async Task Finish()
    {
        if (Deck == null)
        {
            return;
        }

        string? filePath = Deck?.LoadedFilePath;
        if (string.IsNullOrEmpty(filePath))
        {
            var pathTask = GetSavePath?.Invoke();

            if (pathTask == null)
            {
                return;
            }

            filePath = await pathTask;
        }

        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }

        Deck!.Cards.Clear();
        foreach( var cardModel in Cards)
        {
            Deck.Cards.Add(cardModel.CardIndex, new IzeCard()
            {
                CardIndex = cardModel.CardIndex,
                Back = cardModel.BackText,
                Front = cardModel.FrontText
            });
        }

        await Deck!.SaveToFile(filePath);
        Deck = null;

        navigation.NavigateMain(MainWindowView.MainMenu, filePath);
    }

    [RelayCommand]
    public void DeleteCard(ulong cardIndex)
    {
        var card = Cards.FirstOrDefault(x => x.CardIndex == cardIndex);

        if (card != null)
        {
            Cards.Remove(card);
        }
    }

    public Func<Task<string?>>? GetSavePath {get; set;}
}