using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ize.Library;

namespace Ize.Services;

public class PracticeRunService
{

    public static async Task<PracticeRunService> CreateFromPiles(string pilesFilePath)
    {
        var piles = await IzePiles.LoadFromFile(pilesFilePath);
        var deck = await IzeDeck.LoadFromFile(piles.DeckPath);

        return new PracticeRunService(piles.PilesOrder[0], deck, piles);
    }

    public List<CardMetadata> GetActivePile()
    {
        return Piles.Piles[ActivePileName];
    }

    public string ActivePileName {get;}
    public IzePiles Piles {get;}
    public IzeDeck Deck {get;}

    public PracticeRunService(string activePile, IzeDeck deck, IzePiles piles)
    {
        ActivePileName = activePile;
        Piles = piles;
        Deck = deck;

        if (!Piles.Piles.ContainsKey(activePile))
        {
            throw new InvalidOperationException($"Pile {activePile} was not in list of piles.");
        }

        Shuffle();
    }


    public void Shuffle()
    {
        var random = new Random();
        Piles.Piles[ActivePileName].Sort( (_, _) => random.Next(-200, 200));
    }

    public IzeCard? CurrentCard(){
        var activePile = Piles.Piles[ActivePileName];

        if (activePile.Count == 0)
        {
            return null;
        }

        var currentMeta = activePile[^1];

        return Deck.Cards[currentMeta.CardIndex];
    }

    public void MoveTo(string destinationPile)
    {
        var activePile = Piles.Piles[ActivePileName];

        var cardMeta = activePile[^1];
        activePile.RemoveAt(activePile.Count - 1);

        Piles.Piles[destinationPile].Add(cardMeta);
    }

    public void Skip()
    {
        var activePile = Piles.Piles[ActivePileName];

        var cardMeta = activePile[^1];
        activePile.RemoveAt(activePile.Count - 1);
        activePile.Insert(0, cardMeta);
    }

}