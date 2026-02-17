using System.Text.Json;

namespace Ip_Api.Services
{
    public class IpService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public IpService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<JsonDocument> GetIpInfo(string ip)
        {
            var apiKey = _configuration["IpApi:Key"];

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://api.ipregistry.co/{ip}?key={apiKey}");

            var response = await _httpClient.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            return JsonDocument.Parse(content);
        }
    }
}