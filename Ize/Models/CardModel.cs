using CommunityToolkit.Mvvm.ComponentModel;
using Ize.Library;

namespace Ize.Model;

public enum CardSide
{
    Front,
    Back
}

public partial class CardModel : ObservableObject
{
    public CardModel(){}
    public CardModel(IzeCard izeCard)
    {
        FrontText = izeCard.Front;
        BackText = izeCard.Back;
        CardIndex = izeCard.CardIndex;
    }

    [ObservableProperty] string frontText = string.Empty;
    [ObservableProperty] string backText = string.Empty;
    [ObservableProperty] ulong cardIndex;

    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(CurrentText))]
    CardSide currentSide;

    public string CurrentText => CurrentSide == CardSide.Front ? FrontText : BackText;

    public void Flip()
    {
        if (CurrentSide == CardSide.Front)
        {
            CurrentSide = CardSide.Back;
        }
        else
        {
            CurrentSide = CardSide.Front;
        }
    }

}