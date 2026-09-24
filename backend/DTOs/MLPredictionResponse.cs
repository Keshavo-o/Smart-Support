namespace SmartSupport.Api.DTOs;

public record MLPredictionResponse(
    string Category,
    string Priority,
    string Sentiment,
    double Confidence);
