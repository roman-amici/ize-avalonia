
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ize.Library;

public class CardMetadata
{
    public ulong CardIndex { get; set; }

    // Add spaced repetition data times later
}

public class IzePiles
{
    public string OriginalFilePath { get; set; } = string.Empty;
    public string DeckPath { get; set; } = string.Empty;
    public Dictionary<string, List<CardMetadata>> Piles { get; } = new();

    public List<string> PilesOrder = new();

    public Task SaveToFile()
    {
        return SaveToFile(OriginalFilePath);
    }
    public async Task SaveToFile(string filePath)
    {
        using var file = File.CreateText(filePath);

        if (string.IsNullOrEmpty(DeckPath))
        {
            throw new Exception("Deck path cannot be empty.");
        }

        await file.WriteLineAsync(DeckPath);

        foreach (var pileName in PilesOrder)
        {
            await file.WriteLineAsync(pileName);
            var cards = Piles[pileName];

            // Sort to avoid the order changing drastically for each run.
            // We shuffle in the app anyway.
            foreach (var card in cards.OrderBy(x => x.CardIndex))
            {
                await file.WriteLineAsync(card.CardIndex.ToString());
            }
        }
    }


    public static async Task<IzePiles> LoadFromFile(string filePath)
    {
        var piles = new IzePiles()
        {
            OriginalFilePath = filePath
        };

        var readDeckPath = true;
        var currentPile = string.Empty;

        await foreach (var line in File.ReadLinesAsync(filePath))
        {
            var trimmedLine = line.Trim();

            if (string.IsNullOrEmpty(trimmedLine))
            {
                continue;
            }

            if (readDeckPath)
            {
                piles.DeckPath = trimmedLine;
                readDeckPath = false;
                continue;
            }

            if (string.IsNullOrEmpty(currentPile))
            {
                currentPile = trimmedLine;
                piles.PilesOrder.Add(currentPile);
                piles.Piles[currentPile] = [];
                continue;
            }

            var splitLine = trimmedLine.Split("\t");
            // Space repetition data tbd

            if (ulong.TryParse(splitLine[0], out var cardIndex))
            {
                piles.Piles[currentPile].Add(new CardMetadata()
                {
                    CardIndex = cardIndex
                });
            }
            else
            {
                currentPile = trimmedLine;
                piles.PilesOrder.Add(currentPile);
                piles.Piles[currentPile] = [];
            }
        }

        return piles;
    }
}