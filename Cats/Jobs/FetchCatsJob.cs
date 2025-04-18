using Entities.Models;
using Interfaces.Interfaces;

public class FetchCatsJob
{
    private readonly ICatFetcherService _catFetcherService;
    private readonly CatsContext _context;

    public FetchCatsJob(ICatFetcherService catFetcherService, CatsContext context)
    {
        _catFetcherService = catFetcherService;
        _context = context;
    }

    public async Task ExecuteAsync()
    {
        var cats = await _catFetcherService.FetchCatsAsync();

        foreach (var cat in cats)
        {
            if (_context.Cats.Any(c => c.CatId == cat.Id))
                continue;

            //var imageBytes = await _catFetcherService.DownloadImageAsync(cat.Url);

            //var entity = new Cats
            //{
            //    CatId = cat.Id,
            //    Width = cat.Width,
            //    Height = cat.Height,
            //    Image = imageBytes
            //};

            // parse and save tags, if any
            // entity.CatTags = ...

           // _context.Cats.Add(entity);
        }

        //await _context.SaveChangesAsync();
    }
}
