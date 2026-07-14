using System.Net;
using System.Text.Json;

namespace CountryServices;

/// <summary>
/// Provides information about country local currency from RESTful API
/// <see><cref>https://restcountries.com/#api-endpoints-v2</cref></see>.
/// </summary>
public class CountryService : ICountryService
{
    private const string ServiceUrl = "https://restcountries.com/v2";

    private static readonly HttpClient HttpClient = new HttpClient();

    private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
    };

    private readonly Dictionary<string, WeakReference<LocalCurrency>> currencyCountries =
        new Dictionary<string, WeakReference<LocalCurrency>>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Gets information about currency by country code synchronously.
    /// </summary>
    /// <param name="alpha2Or3Code">ISO 3166-1 2-letter or 3-letter country code.</param>
    /// <see><cref>https://en.wikipedia.org/wiki/List_of_ISO_3166_country_codes</cref></see>
    /// <returns>Information about country currency as <see cref="LocalCurrency"/>.</returns>
    /// <exception cref="ArgumentException">Throw if countryCode is null, empty, whitespace or invalid country code.</exception>
    public LocalCurrency GetLocalCurrencyByAlpha2Or3Code(string? alpha2Or3Code)
    {
        string code = ValidateRequired(alpha2Or3Code, nameof(alpha2Or3Code)).ToUpperInvariant();

        if (this.currencyCountries.TryGetValue(code, out WeakReference<LocalCurrency>? weakReference) &&
            weakReference.TryGetTarget(out LocalCurrency? cachedCurrency))
        {
            return cachedCurrency;
        }

        Uri requestUri = new Uri($"{ServiceUrl}/alpha/{Uri.EscapeDataString(code)}", UriKind.Absolute);

        try
        {
#pragma warning disable SYSLIB0014
            using WebClient webClient = new WebClient();
            string json = webClient.DownloadString(requestUri);
#pragma warning restore SYSLIB0014

            LocalCurrencyInfo response = DeserializeRequired<LocalCurrencyInfo>(json);
            LocalCurrency result = MapLocalCurrency(response);

            this.currencyCountries[code] = new WeakReference<LocalCurrency>(result);
            return result;
        }
        catch (WebException)
        {
            throw CreateArgumentException(nameof(alpha2Or3Code));
        }
        catch (JsonException)
        {
            throw CreateArgumentException(nameof(alpha2Or3Code));
        }
    }

    /// <summary>
    /// Gets information about currency by country code asynchronously.
    /// </summary>
    /// <param name="alpha2Or3Code">ISO 3166-1 2-letter or 3-letter country code.</param>
    /// <see><cref>https://en.wikipedia.org/wiki/List_of_ISO_3166_country_codes</cref></see>.
    /// <param name="token">Token for cancellation asynchronous operation.</param>
    /// <returns>Information about country currency as <see cref="LocalCurrency"/>.</returns>
    /// <exception cref="ArgumentException">Throw if countryCode is null, empty, whitespace or invalid country code.</exception>
    public async Task<LocalCurrency> GetLocalCurrencyByAlpha2Or3CodeAsync(string? alpha2Or3Code, CancellationToken token)
    {
        string code = ValidateRequired(alpha2Or3Code, nameof(alpha2Or3Code)).ToUpperInvariant();

        if (this.currencyCountries.TryGetValue(code, out WeakReference<LocalCurrency>? weakReference) &&
            weakReference.TryGetTarget(out LocalCurrency? cachedCurrency))
        {
            return cachedCurrency;
        }

        Uri requestUri = new Uri($"{ServiceUrl}/alpha/{Uri.EscapeDataString(code)}", UriKind.Absolute);

        try
        {
            using HttpResponseMessage response = await HttpClient.GetAsync(requestUri, token);

            if (!response.IsSuccessStatusCode)
            {
                throw CreateArgumentException(nameof(alpha2Or3Code));
            }

            await using Stream stream = await response.Content.ReadAsStreamAsync(token);
            LocalCurrencyInfo? countryResponse =
                await JsonSerializer.DeserializeAsync<LocalCurrencyInfo>(stream, JsonSerializerOptions, token);

            if (countryResponse is null)
            {
                throw CreateArgumentException(nameof(alpha2Or3Code));
            }

            LocalCurrency result = MapLocalCurrency(countryResponse);

            this.currencyCountries[code] = new WeakReference<LocalCurrency>(result);
            return result;
        }
        catch (HttpRequestException)
        {
            throw CreateArgumentException(nameof(alpha2Or3Code));
        }
        catch (JsonException)
        {
            throw CreateArgumentException(nameof(alpha2Or3Code));
        }
    }

    /// <summary>
    /// Gets information about the country by the country capital synchronously.
    /// </summary>
    /// <param name="capital">Capital name.</param>
    /// <returns>Information about the country as <see cref="Country"/>.</returns>
    /// <exception cref="ArgumentException">Throw if the capital name is null, empty, whitespace or nonexistent.</exception>
    public Country GetCountryInfoByCapital(string? capital)
    {
        string capitalName = ValidateRequired(capital, nameof(capital));
        Uri requestUri = new Uri($"{ServiceUrl}/capital/{Uri.EscapeDataString(capitalName)}", UriKind.Absolute);

        try
        {
#pragma warning disable SYSLIB0014
            using WebClient webClient = new WebClient();
            string json = webClient.DownloadString(requestUri);
#pragma warning restore SYSLIB0014

            CountryInfo[] responses = DeserializeRequired<CountryInfo[]>(json);
            CountryInfo countryResponse = GetFirstOrThrow(responses, nameof(capital));

            return MapCountry(countryResponse);
        }
        catch (WebException)
        {
            throw CreateArgumentException(nameof(capital));
        }
        catch (JsonException)
        {
            throw CreateArgumentException(nameof(capital));
        }
    }

    /// <summary>
    /// Gets information about the currency by the country capital asynchronously.
    /// </summary>
    /// <param name="capital">Capital name.</param>
    /// <param name="token">Token for cancellation asynchronous operation.</param>
    /// <returns>Information about the country as <see cref="Country"/>.</returns>
    /// <exception cref="ArgumentException">Throw if the capital name is null, empty, whitespace or nonexistent.</exception>
    public async Task<Country> GetCountryInfoByCapitalAsync(string? capital, CancellationToken token)
    {
        string capitalName = ValidateRequired(capital, nameof(capital));
        Uri requestUri = new Uri($"{ServiceUrl}/capital/{Uri.EscapeDataString(capitalName)}", UriKind.Absolute);

        try
        {
            using HttpResponseMessage response = await HttpClient.GetAsync(requestUri, token);

            if (!response.IsSuccessStatusCode)
            {
                throw CreateArgumentException(nameof(capital));
            }

            await using Stream stream = await response.Content.ReadAsStreamAsync(token);
            CountryInfo[] responses =
                await JsonSerializer.DeserializeAsync<CountryInfo[]>(stream, JsonSerializerOptions, token)
                ?? Array.Empty<CountryInfo>();

            CountryInfo countryResponse = GetFirstOrThrow(responses, nameof(capital));
            return MapCountry(countryResponse);
        }
        catch (HttpRequestException)
        {
            throw CreateArgumentException(nameof(capital));
        }
        catch (JsonException)
        {
            throw CreateArgumentException(nameof(capital));
        }
    }

    private static string ValidateRequired(string? value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw CreateArgumentException(paramName);
        }

        return value.Trim();
    }

    private static ArgumentException CreateArgumentException(string paramName)
    {
        return new ArgumentException("Value is invalid.", paramName);
    }

    private static T DeserializeRequired<T>(string json)
    {
        T? result = JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);

        if (result is null)
        {
            throw new JsonException("Response body is empty.");
        }

        return result;
    }

    private static T GetFirstOrThrow<T>(T[] items, string paramName)
    {
        if (items.Length == 0)
        {
            throw CreateArgumentException(paramName);
        }

        return items[0];
    }

    private static LocalCurrency MapLocalCurrency(LocalCurrencyInfo response)
    {
        LocalCurrencyInfo.Currency? currency = response.Currencies.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(response.CountryName) || currency is null || string.IsNullOrWhiteSpace(currency.Code))
        {
            throw CreateArgumentException("alpha2Or3Code");
        }

        return new LocalCurrency
        {
            CountryName = response.CountryName,
            CurrencyCode = currency.Code,
            CurrencySymbol = currency.Symbol,
        };
    }

    private static Country MapCountry(CountryInfo response)
    {
        if (string.IsNullOrWhiteSpace(response.Name) || string.IsNullOrWhiteSpace(response.CapitalName))
        {
            throw CreateArgumentException("capital");
        }

        return new Country
        {
            Name = response.Name,
            CapitalName = response.CapitalName,
            Area = response.Area,
            Population = response.Population,
            Flag = response.Flag,
        };
    }
}
