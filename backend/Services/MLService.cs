using System.Net.Http.Json;
using SmartSupport.Api.DTOs;

namespace SmartSupport.Api.Services;

public class MLService : IMLService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<MLService> _logger;

    public MLService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<MLService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<MLPredictionResponse> PredictAsync(string text)
    {
        var baseUrl = _configuration["MLService:BaseUrl"]?.TrimEnd('/');

        if (string.IsNullOrWhiteSpace(baseUrl))
            return FallbackPrediction();

        try
        {
            var client = _httpClientFactory.CreateClient("ML");
            var response = await client.PostAsJsonAsync(
                $"{baseUrl}/predict",
                new { text });

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("ML service returned {StatusCode}", response.StatusCode);
                return FallbackPrediction();
            }

            var prediction = await response.Content
                .ReadFromJsonAsync<MLPredictionResponse>();

            return prediction ?? FallbackPrediction();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "ML service unavailable. Using fallback prediction.");
            return FallbackPrediction();
        }
    }

    private static MLPredictionResponse FallbackPrediction() =>
        new("Unclassified", "Medium", "Unknown", 0);
}
