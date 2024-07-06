using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ize.Model;

public partial class PileModel : ObservableObject
{
    [ObservableProperty] char? hotKey;
    [ObservableProperty] string pileName = string.Empty;
    [ObservableProperty] int remainingCards;


}