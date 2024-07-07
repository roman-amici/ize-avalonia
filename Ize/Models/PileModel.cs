using System.Collections.ObjectModel;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ize.Model;

public partial class PileModel : ObservableObject
{
    [ObservableProperty] Key hotKey = Key.None;
    [ObservableProperty] string displayHotKey = string.Empty;
    [ObservableProperty] string pileName = string.Empty;
    [ObservableProperty] int remainingCards;


    public static Key NumberKey(int i)
    {
        return i switch
        {
            1 => Key.NumPad1,
            2 => Key.NumPad2,
            3 => Key.NumPad3,
            4 => Key.NumPad4,
            5 => Key.NumPad5,
            6 => Key.NumPad6,
            7 => Key.NumPad7,
            8 => Key.NumPad8,
            9 => Key.NumPad9,
            _ => Key.None
        };
    }
}