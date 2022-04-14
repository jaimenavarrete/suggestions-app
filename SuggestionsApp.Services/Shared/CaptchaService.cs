using System.Text.Json;
using SuggestionsApp.Models.Responses;

namespace SuggestionsApp.Services.Shared;

public static class CaptchaService
{
    private const string Url = "https://www.google.com/recaptcha/api/siteverify";
    private const string Secret = "6Ld_7W8fAAAAAOktQzP5tbMg-tpTK1l5ZsKn_gFe";

    public static async Task<bool> ValidateCaptchaToken(string token)
    {
        var httpClient = new HttpClient();

        var values = new Dictionary<string, string>
        {
            { "secret", Secret },
            { "response", token }
        };

        var parameters = new FormUrlEncodedContent(values);
        
        var response = await httpClient.PostAsync(Url, parameters);
        var text = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<CaptchaResponse>(text);

        return result is not null && result.Success;
    }
}