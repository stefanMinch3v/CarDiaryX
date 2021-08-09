using CarDiaryX.Application.Contracts;
using CarDiaryX.Domain.Integration;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CarDiaryX.Integration
{
    public class VehicleHttpService : IVehicleHttpService
    {
        private const string BASE_URL = "http://nrpla.de";
        private readonly IMemoryCache memoryCache;
        private readonly MemoryCacheEntryOptions cacheOptions;
        private readonly HttpClient httpClient;

        public VehicleHttpService(HttpClient httpClient, IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
            this.cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(relative: TimeSpan.FromSeconds(20_000)); // 21600 expires the api cookie

            httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            httpClient.DefaultRequestHeaders.Add("Host", "nrpla.de");
            httpClient.DefaultRequestHeaders.Add("Refer", "http://nrpla.de/");
            httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
            this.httpClient = httpClient;
        }

        public async Task<RootDMR> GetDMR(long tsId, CancellationToken cancellationToken)
        {
            await this.AddDefaultAuthCookieRequest(cancellationToken);

            var response = await this.httpClient.GetAsync($"{BASE_URL}/dmr?ts_id={tsId}", cancellationToken);
            var content = await response.Content.ReadAsStringAsync();

            var root = JsonConvert.DeserializeObject<RootDMR>(content);
            root.RawData = content;

            return root;
        }

        public async Task<RootInformation> GetInformation(string plates, CancellationToken cancellationToken)
        {
            await this.AddDefaultAuthCookieRequest(cancellationToken);

            var response = await this.httpClient.GetAsync($"{BASE_URL}/data?registration={plates}&mode=license&kid=", cancellationToken);
            var content = await response.Content.ReadAsStringAsync();
            
            var root = JsonConvert.DeserializeObject<RootInformation>(content);
            root.RawData = content;

            return root;
        }

        public async Task<string> GetInspections(long id, CancellationToken cancellationToken)
        {
            await this.AddDefaultAuthCookieRequest(cancellationToken);

            var response = await this.httpClient.GetAsync($"{BASE_URL}/inspections/{id}", cancellationToken);
            return await response.Content.ReadAsStringAsync();
        }

        private async Task AddDefaultAuthCookieRequest(CancellationToken cancellationToken)
        {
            var cookie = await this.memoryCache.GetOrCreateAsync("AuthCookie", async entry =>
            {
                entry.SetOptions(this.cacheOptions);

                var response = await this.httpClient.GetAsync(BASE_URL, cancellationToken);
                response.EnsureSuccessStatusCode();

                var cookies = response.Headers.FirstOrDefault(r => r.Key == "Set-Cookie").Value;
                var cookie = string.Join(';', cookies.Select(c => c.Split(';').FirstOrDefault()));

                return cookie;
            });

            const string cookieName = "Cookie";
            this.httpClient.DefaultRequestHeaders.Remove(cookieName);
            this.httpClient.DefaultRequestHeaders.Add(cookieName, cookie);
        }
    }
}
