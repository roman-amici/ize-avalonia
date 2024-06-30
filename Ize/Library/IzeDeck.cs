using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace Ize.Library;

public class IzeCard
{
    public ulong CardIndex { get; set; }
    public string Front { get; set; } = string.Empty;
    public string Back { get; set; } = string.Empty;
}

public class IzeDeck
{
    public string? LoadedFilePath { get; private set; } = null;
    public Dictionary<ulong, IzeCard> Cards { get; set; } = new();

    public static async Task<IzeDeck> LoadFromFile(string filePath)
    {
        var deck = new IzeDeck();

        IzeCard? nextCard = null;
        await foreach (var line in File.ReadLinesAsync(filePath))
        {
            var trimmedLine = line.Trim();

            if (nextCard == null)
            {
                // Arbitrary whitespace allowed between cards
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (!ulong.TryParse(trimmedLine, out var cardIndex))
                {
                    throw new Exception($"Invalid deck file. Expected card id but found, {trimmedLine}");
                }
                nextCard = new()
                {
                    CardIndex = cardIndex
                };
            }
            else if (string.IsNullOrEmpty(nextCard.Front))
            {
                if (string.IsNullOrEmpty(trimmedLine))
                {
                    throw new Exception("Invalid deck file. Card front was empty");
                }
                nextCard.Front = trimmedLine;
            }
            else if (string.IsNullOrEmpty(nextCard.Back))
            {
                if (string.IsNullOrEmpty(trimmedLine))
                {
                    throw new Exception("Invalid deck file. Card back was empty");
                }
                nextCard.Back = trimmedLine;

                deck.Cards.Add(nextCard.CardIndex, nextCard);
                nextCard = null;
            }
        }

        return deck;
    }
}