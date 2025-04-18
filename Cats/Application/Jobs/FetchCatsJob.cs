using AutoMapper;
using Interfaces.Interfaces;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;

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
                // Fetch the list of cats from the external API
                var cats = await _catFetcherService.FetchCatsAsync();

                // Fetch existing cat IDs from the database to prevent duplicates
                var existingCatIds = new HashSet<string>(await _unitOfWork.CatRepository.GetExistingCatIdsAsync());

                // Create a cache for tags to prevent duplicate tag creation
                var tagCache = new Dictionary<string, Tag>(StringComparer.OrdinalIgnoreCase);

                // Preload tags from DB into the cache
                var existingTags = await _unitOfWork.TagRepository.GetAllTagsAsync();
                foreach (var tag in existingTags)
                {
                    tagCache[tag.Name] = tag;
                }

                foreach (var cat in cats)
                {
                    if (existingCatIds.Contains(cat.Id))
                        continue; // Skip if the cat already exists

                    try
                    {
                        // Download the cat image
                        var imageBytes = await _catFetcherService.DownloadImageAsync(cat.Url);

                        // Use AutoMapper to map CatDto to Cat entity
                        var dbCat = _mapper.Map<Cat>(cat);
                        dbCat.Image = imageBytes;

                        // Process tags and associate with the new cat using AutoMapper for breeds
                        var tags = _mapper.Map<List<Tag>>(cat.Breeds ?? new List<BreedDto>());

                        foreach (var tag in tags)
                        {
                            Tag tagFromCache;

                            // Check if the tag is already in the cache
                            if (!tagCache.TryGetValue(tag.Name, out tagFromCache))
                            {
                                // Add new tag if not found in cache
                                await _unitOfWork.TagRepository.AddTagAsync(tag);
                                tagCache[tag.Name] = tag; // Cache the new tag
                            }

                            // Associate the tag with the cat
                            dbCat.Tags.Add(tag);
                        }

                        // Add the new cat to the repository
                        await _unitOfWork.CatRepository.AddCatAsync(dbCat);
                    }
                    catch (Exception ex)
                    {
                        // Log error for this specific cat
                        Console.WriteLine($"Error processing cat {cat.Id}: {ex.Message}");
                    }
                }

                // Save all changes (cats and tags) in one transaction
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching cats: {ex.Message}");
            }
        }
    }
}
