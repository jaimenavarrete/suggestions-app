using System.Text.Json.Serialization;

namespace SuggestionsApp.Models.Responses;

public class CaptchaResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("challenge_ts")]
    public DateTime ChallengeTs { get; set; }

    [JsonPropertyName("hostname")]
    public string Hostname { get; set; } = null!;

    [JsonPropertyName("error-codes")] 
    public string[] ErrorCodes { get; set; } = null!;
}