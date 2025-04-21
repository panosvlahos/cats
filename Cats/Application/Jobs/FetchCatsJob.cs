using AutoMapper;
using Interfaces.Interfaces;
using Entities.Models;
using Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class FetchCatsJob
    {
        private readonly ICatFetcherService _catFetcherService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FetchCatsJob(ICatFetcherService catFetcherService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _catFetcherService = catFetcherService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                var existingCatIds = new HashSet<string>(
                    await _unitOfWork.CatRepository.GetExistingCatIdsAsync());

                var tagCache = new ConcurrentDictionary<string, Tag>(StringComparer.OrdinalIgnoreCase);
                var existingTags = await _unitOfWork.TagRepository.GetAllTagsAsync();

                foreach (var tag in existingTags)
                {
                    tagCache[tag.Name] = tag;
                }

                var cats = await _catFetcherService.FetchCatsAsync();
                if (cats == null || cats.Count == 0) return;

                var newCats = new ConcurrentBag<Cat>();
                var newTags = new ConcurrentDictionary<string, Tag>();

                var tasks = cats
                    .Where(c => !existingCatIds.Contains(c.Id))
                    .Select(async catDto =>
                    {
                        try
                        {
                            var imageBytes = await _catFetcherService.DownloadImageAsync(catDto.Url);

                            var dbCat = _mapper.Map<Cat>(catDto);
                            dbCat.Image = imageBytes;
                            dbCat.Tags = new List<Tag>();

                            if (catDto.Breeds is { Count: > 0 })
                            {
                                var tags = _mapper.Map<List<Tag>>(catDto.Breeds);

                                foreach (var tag in tags)
                                {
                                    var cachedTag = tagCache.GetOrAdd(tag.Name, name =>
                                    {
                                        newTags[name] = tag;
                                        return tag;
                                    });

                                    dbCat.Tags.Add(cachedTag);
                                }
                            }

                            newCats.Add(dbCat);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error processing cat {catDto.Id}: {ex.Message}");
                        }
                    });

                await Task.WhenAll(tasks);

                if (newTags.Count > 0)
                    await _unitOfWork.TagRepository.AddTagAsync(newTags.Values.ToList());

                if (newCats.Count > 0)
                    await _unitOfWork.CatRepository.AddCatAsync(newCats.ToList());

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching cats: {ex.Message}");
            }
        }
    }
}
