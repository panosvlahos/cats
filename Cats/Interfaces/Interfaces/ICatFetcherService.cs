namespace Interfaces.Interfaces
{
    public interface ICatFetcherService
    {
        Task<List<CatDto>> FetchCatsAsync(int count = 25);
        Task<byte[]> DownloadImageAsync(string imageUrl);
    }
}
