using Contracts;
using Infrastructure.Configuration;
using Interfaces.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Services.Services
{
    public class CatApiService : ICatFetcherService
    {
        private readonly HttpClient _httpClient;
        private readonly TheCatApiOptions _apiOptions;
        private readonly TheCatApiOptions _options;
        public CatApiService(HttpClient httpClient, IOptions<TheCatApiOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<List<CatDto>> FetchCatsAsync(int count = 25)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_options.BaseUrl}/images/search?limit=25&has_breeds=1");

                // Add API key header here
                request.Headers.Add("x-api-key", _options.ApiKey);

                var response = await _httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                
                var cats = JsonConvert.DeserializeObject<List<CatDto>>(content);
                return cats!;
            }
            catch (Exception ex)
            {
                // Handle HTTP-specific exceptions
                Console.WriteLine(ex.Message.ToString());
                return new List<CatDto>(); // Return an empty list or rethrow if needed
            }
           
        }
        public async Task<byte[]> DownloadImageAsync(string imageUrl)
        {
            return await _httpClient.GetByteArrayAsync(imageUrl);
        }
    }
}
