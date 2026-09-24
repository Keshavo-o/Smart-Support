using SmartSupport.Api.DTOs;

namespace SmartSupport.Api.Services;

public interface IMLService
{
    Task<MLPredictionResponse> PredictAsync(string text);
}
