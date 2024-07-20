using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ize.Library;
using Ize.Model;

namespace Ize.ViewModels;

public partial class DeckEditorViewModel : ObservableObject
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
    public void Finish()
    {

    }
}