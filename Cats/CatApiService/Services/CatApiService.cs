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
            var url = $"https://api.thecatapi.com/v1/images/search?limit={count}&has_breeds=1";
            var response = await _httpClient.GetStringAsync(url);
            var cats = JsonConvert.DeserializeObject<List<CatDto>>(response);
            return cats!;
        }
    }
}
