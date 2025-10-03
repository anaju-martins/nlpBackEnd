using nplBackEnd.DTOs;
using nplBackEnd.Services.Abstractions;
using System.Text.Json;

namespace nplBackEnd.Services.Implementations;
    public class NlpService : INlpService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<NlpService> _logger;

        public NlpService(IHttpClientFactory httpClientFactory, ILogger<NlpService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<NlpResponse?> AnalyzeTextAsync(NlpRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("NlpService");

            try
            {
                var response = await httpClient.PostAsJsonAsync("analyze", request);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error calling NLP service. Status: {StatusCode}, Response: {Response}",
                        response.StatusCode, await response.Content.ReadAsStringAsync());
                    return null;
                }

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return await response.Content.ReadFromJsonAsync<NlpResponse>(options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while calling the NLP service.");
                return null;
            }
        }
    }
