using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Ize.Services;

public class RecentFileService
{
    public string FilePath { get; set; } = string.Empty;
    public List<string> FilePaths { get; set; } = [];

    public void SaveToFile()
    {
        File.WriteAllText(FilePath, JsonSerializer.Serialize(FilePaths));
    }

    public static RecentFileService LoadFromFile(string filePath)
    {
        var service = new RecentFileService()
        {
            FilePath = filePath
        };

        if (!Path.Exists(filePath))
        {
            return service;
        }

        var paths = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(filePath));
        if (paths != null)
        {
            service.FilePaths = paths;
        }

        return service;
    }
}