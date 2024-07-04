namespace Ize.Services;

public class ServicesCollection
{
    public ServicesCollection()
    {
        RecentFileService = RecentFileService.LoadFromFile("./recents.json");
    }

    public RecentFileService RecentFileService {get;}
}