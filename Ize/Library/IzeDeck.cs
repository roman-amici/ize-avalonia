using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace Ize.Library;

public class IzeDeck
{
    public string? LoadedFilePath { get; private set; } = null;
    public Dictionary<ulong, IzeCard> Cards { get; set; } = new();
    public string? Name => string.IsNullOrEmpty(LoadedFilePath) ? null : Path.GetFileNameWithoutExtension(LoadedFilePath); 

    public static async Task<IzeDeck> LoadFromFile(string filePath)
    {
        var deck = new IzeDeck();
        deck.LoadedFilePath = Path.GetFullPath(filePath);

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

    public async Task SaveToFile(string filePath)
    {
        using var file = File.CreateText(filePath);

        foreach (var card in Cards)
        {
            await file.WriteLineAsync(card.Key.ToString());
            await file.WriteLineAsync(card.Value.Front.ToString());
            await file.WriteLineAsync(card.Value.Back.ToString());
            await file.WriteLineAsync();
        }
        
    }
}