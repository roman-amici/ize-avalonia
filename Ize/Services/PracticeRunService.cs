using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ize.Library;

namespace Ize.Services;

public class PracticeRunService
{
	private static readonly string[] defaultPiles = ["remaining", "working", "incorrect", "memorized"];

	public PracticeRunService(string activePile, IzeDeck deck, IzePiles piles)
	{
		ActivePileName = activePile;
		Piles = piles;
		Deck = deck;

		if (!Piles.Piles.ContainsKey(activePile))
		{
			throw new InvalidOperationException($"Pile {activePile} was not in list of piles.");
		}

		var cardsInPile = new HashSet<ulong>();
		foreach (var pile in piles.Piles)
		{
			foreach (var card in pile.Value)
			{
				cardsInPile.Add(card.CardIndex);
			}
		}

		var activePileList = GetActivePile();
		foreach (var cardIndex in deck.Cards.Keys)
		{
			if (!cardsInPile.Contains(cardIndex))
			{
				activePileList.Add(new CardMetadata
				{
					CardIndex = cardIndex
				});
			}
		}

		Shuffle();
	}

	public string ActivePileName { get; }
	public IzePiles Piles { get; }
	public IzeDeck Deck { get; }

	public static async Task<PracticeRunService> CreateFromPiles(string pilesFilePath)
	{
		var piles = await IzePiles.LoadFromFile(pilesFilePath);
		var deck = await IzeDeck.LoadFromFile(piles.DeckPath);

		return new PracticeRunService(piles.PilesOrder[0], deck, piles);
	}

	public static async Task<PracticeRunService> CreateFromDeck(string deckFilePath)
	{
		var deck = await IzeDeck.LoadFromFile(deckFilePath);
		var piles = new IzePiles(defaultPiles, deck)
		{
			DeckPath = deck.LoadedFilePath!
		};

		return new PracticeRunService(piles.PilesOrder.First(), deck, piles);
	}

	public List<CardMetadata> GetActivePile()
	{
		return Piles.Piles[ActivePileName];
	}

	public void MoveToActive(string destinationPileName)
	{
		if (destinationPileName == ActivePileName)
		{
			return;
		}

		var destinationPile = Piles.Piles[destinationPileName];
		var activePile = GetActivePile();
		activePile.AddRange(destinationPile);
		destinationPile.Clear();
	}

	public void Shuffle()
	{
		var random = new Random();
		GetActivePile().Sort((_, _) => random.Next(-200, 200));
	}

	public IzeCard? CurrentCard()
	{
		var activePile = GetActivePile();

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

		if (activePile.Count == 0)
		{
			return;
		}

		var cardMeta = activePile[^1];
		activePile.RemoveAt(activePile.Count - 1);

		Piles.Piles[destinationPile].Add(cardMeta);
	}

	public void Skip()
	{
		var activePile = Piles.Piles[ActivePileName];

		if (activePile.Count == 0)
		{
			return;
		}

		var cardMeta = activePile[^1];
		activePile.RemoveAt(activePile.Count - 1);
		activePile.Insert(0, cardMeta);
	}
}