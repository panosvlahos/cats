using Entities.Models;
using Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        try
        {
            var cats = await _catFetcherService.FetchCatsAsync();

            var existingCatIds = new HashSet<string>(
                await _context.Cat.Select(c => c.CatId).ToListAsync()
            );

            // 🔒 Cache of tags already seen or created in this run
            var tagCache = new Dictionary<string, Tag>(StringComparer.OrdinalIgnoreCase);

            // Preload tags from DB
            foreach (var tag in await _context.Tag.ToListAsync())
            {
                tagCache[tag.Name] = tag;
            }

            foreach (var cat in cats)
            {
                if (existingCatIds.Contains(cat.Id) ||
                    _context.ChangeTracker.Entries<Cat>().Any(e => e.Entity.CatId == cat.Id))
                    continue;

                try
                {
                    var imageBytes = await _catFetcherService.DownloadImageAsync(cat.Url);

                    var dbCat = new Cat
                    {
                        CatId = cat.Id,
                        Width = cat.Width,
                        Height = cat.Height,
                        Image = imageBytes
                    };

                    var tags = (cat.Breeds ?? new List<BreedDto>())
                        .Where(b => !string.IsNullOrWhiteSpace(b.Temperament))
                        .SelectMany(b => b.Temperament.Split(',', StringSplitOptions.RemoveEmptyEntries))
                        .Select(t => t.Trim())
                        .Distinct();

                    foreach (var tagName in tags)
                    {
                        if (!tagCache.TryGetValue(tagName, out var tag))
                        {
                            tag = new Tag { Name = tagName };
                            tagCache[tagName] = tag;
                        }

                        dbCat.Tags.Add(tag);
                    }

                    _context.Cat.Add(dbCat);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing cat {cat.Id}: {ex.Message}");
                }
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching cats: {ex.Message}");
        }
    }


}
