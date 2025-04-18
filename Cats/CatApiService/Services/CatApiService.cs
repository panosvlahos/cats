using Interfaces.Interfaces;
using Newtonsoft.Json;

namespace Services.Services
{
    public class CatApiService : ICatFetcherService
    {
        private readonly HttpClient _httpClient;

        public CatApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CatDto>> FetchCatsAsync(int count = 25)
        {
            try
            {
                var url = $"https://api.thecatapi.com/v1/images/search?limit={count}";

                // Add API key to the request headers
                _httpClient.DefaultRequestHeaders.Add("x-api-key", "live_Oh0Ch2hH9WBHu3EMiZvBRBMfsqoVHcfH8wpjH4jKg3YtiVhIlOiyxzboN9DcDlx7");

                var response = await _httpClient.GetStringAsync(url);

                var cats = JsonConvert.DeserializeObject<List<CatDto>>(response);
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
