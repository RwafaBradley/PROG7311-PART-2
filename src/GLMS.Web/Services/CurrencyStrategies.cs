// Learning edition: every section is explained inline so the code reads like a guided walkthrough.
// Think of the app like a logistics office: controllers are the reception desk, services are the back office,
// models are the forms, and the in-memory store is the filing cabinet while the database is still simulated.

// Declares the namespace so the class belongs to the GLMS.Web area.
using System.Text.Json;

namespace GLMS.Web.Services;

// Defines a contract that other classes must follow.
public interface ICurrencyConversionStrategy
// Marks the beginning or end of a block in C#.
{
    decimal GetUsdToZarRate();
    decimal ConvertUsdToZar(decimal usdAmount);
    string StrategyName { get; }
// Marks the beginning or end of a block in C#.
}

// Defines a class, which groups related data or behavior together.
public class FixedRateCurrencyStrategy : ICurrencyConversionStrategy
// Marks the beginning or end of a block in C#.
{
    // Stores a dependency that the class will reuse without re-creating it.
    private readonly decimal _rate;

    // Declares a method or property that other parts of the app can use.
    public FixedRateCurrencyStrategy(IConfiguration configuration)
    // Marks the beginning or end of a block in C#.
    {
        // Uses an existing field or dependency rather than creating a duplicate copy.
        _rate = configuration.GetValue<decimal>("ExternalApis:DefaultUsdToZarRate", 18.50m);
    // Marks the beginning or end of a block in C#.
    }

    public string StrategyName => "Fixed placeholder rate";

    public decimal GetUsdToZarRate() => _rate;

    public decimal ConvertUsdToZar(decimal usdAmount) => Math.Round(usdAmount * _rate, 2, MidpointRounding.AwayFromZero);
    // Marks the beginning or end of a block in C#.
}

// Defines a class, which groups related data or behavior together.
// Learning edition: every section is explained inline so the code reads like a guided walkthrough.
// Think of the app like a logistics office: controllers are the reception desk, services are the back office,
// models are the forms, and the in-memory store is the filing cabinet while the database is still simulated.


// Defines a class, which groups related data or behavior together.
public class PlaceholderApiCurrencyStrategy : ICurrencyConversionStrategy
{
    // Stores a dependency that the class will reuse without re-creating it.
    private readonly IConfiguration _configuration;

    // Declares a method or property that other parts of the app can use.
    public PlaceholderApiCurrencyStrategy(IConfiguration configuration)
    {
        // Uses an existing field or dependency rather than creating a duplicate copy.
        _configuration = configuration;
    }

    public string StrategyName => "Live API currency strategy";

    // Declares a method or property that other parts of the app can use.
    public decimal GetUsdToZarRate()
    {
        // Reads the API base URL from appsettings.json.
        var baseUrl = _configuration["ExternalApis:CurrencyApiBaseUrl"];

        // Reads the API key from appsettings.json.
        var apiKey = _configuration["ExternalApis:CurrencyApiKey"] ?? string.Empty;

        // Reads the fallback rate from appsettings.json.
        var fallbackRate = _configuration.GetValue<decimal>("ExternalApis:DefaultUsdToZarRate", 18.50m);

        // Checks a rule before continuing, so invalid states are stopped early.
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            return fallbackRate;
        }

        // Builds the final URL.
        // This supports placeholders in your config like {API_KEY}, {FROM}, and {TO}.
        var url = baseUrl
            .Replace("{API_KEY}", Uri.EscapeDataString(apiKey))
            .Replace("{FROM}", "USD")
            .Replace("{TO}", "ZAR");

        try
        {
            // Creates a client to call the API.
            using var client = new HttpClient();

            // Calls the API and waits for the response.
            var json = client.GetStringAsync(url).GetAwaiter().GetResult();

            // Parses the JSON response.
            using var document = JsonDocument.Parse(json);

            // Tries to read the exchange rate from a few common API response shapes.
            if (TryReadZarRate(document.RootElement, out var rate))
            {
                return rate;
            }

            // If the API response shape is not what we expected, use the fallback.
            return fallbackRate;
        }
        catch
        {
            // If the API fails, the app still works using the placeholder rate.
            return fallbackRate;
        }
    }

    // Declares a method or property that other parts of the app can use.
    public decimal ConvertUsdToZar(decimal usdAmount)
    {
        // Creates a variable and lets C# infer the type from the right-hand side.
        var rate = GetUsdToZarRate();

        // Sends a value back to the caller and ends this branch of logic.
        return Math.Round(usdAmount * rate, 2, MidpointRounding.AwayFromZero);
    }

    // Tries to read the ZAR rate from common JSON structures.
    private static bool TryReadZarRate(JsonElement root, out decimal rate)
    {
        rate = 0m;

        // Common format 1:
        // { "rates": { "ZAR": 18.45 } }
        if (root.TryGetProperty("rates", out var ratesElement) &&
            ratesElement.ValueKind == JsonValueKind.Object &&
            ratesElement.TryGetProperty("ZAR", out var zar1) &&
            zar1.TryGetDecimal(out rate))
        {
            return true;
        }

        // Common format 2:
        // { "conversion_rates": { "ZAR": 18.45 } }
        if (root.TryGetProperty("conversion_rates", out var conversionRatesElement) &&
            conversionRatesElement.ValueKind == JsonValueKind.Object &&
            conversionRatesElement.TryGetProperty("ZAR", out var zar2) &&
            zar2.TryGetDecimal(out rate))
        {
            return true;
        }

        // Common format 3:
        // { "data": { "ZAR": 18.45 } }
        if (root.TryGetProperty("data", out var dataElement) &&
            dataElement.ValueKind == JsonValueKind.Object &&
            dataElement.TryGetProperty("ZAR", out var zar3) &&
            zar3.TryGetDecimal(out rate))
        {
            return true;
        }

        // Common format 4:
        // { "ZAR": 18.45 }
        if (root.TryGetProperty("ZAR", out var directZar) &&
            directZar.TryGetDecimal(out rate))
        {
            return true;
        }

        return false;
    }
}
