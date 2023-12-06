using ExchangeAPI.Modules.Identity.Application.Common.Interfaces.Services;
using ExchangeAPI.Modules.Identity.Shared.DTOs;
using ExchangeAPI.Shared.Common.Constants;
using ExchangeAPI.Shared.Common.Models.Result.Abstractions.Generics;
using ExchangeAPI.Shared.Common.Models.Result.Implementations.Generics;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace ExchangeAPI.Modules.Identity.Infrastructure.Services;

internal class OpenExchangeRatesService : IOpenExchangeRatesService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public OpenExchangeRatesService(HttpClient httpClient, IConfiguration configuration)
    {
        _jsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(configuration["OpenExchangeRate:ApiUrl"]
                                          ?? throw new ArgumentNullException(nameof(_httpClient.BaseAddress), "OpenExchangeRate:ApiUrl"));

        _apiKey = configuration["OpenExchangeRate:ApiKey"]
                  ?? throw new ArgumentNullException(nameof(_apiKey), "OpenExchangeRate:ApiKey");
    }

    public async Task<IResult<OpenExchangeResponse>> ConvertAsync(decimal value, string from, string to)
    {
        var uri = $"convert/{value}/{from.ToUpper()}/{to.ToUpper()}?app_id={_apiKey}";

        var httpResponseMessage = await _httpClient.GetAsync(uri);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            await using var errorStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            var errorResult = JsonSerializer.Deserialize<OpenExchangeRateError>(errorStream, _jsonSerializerOptions);

            return Result<OpenExchangeResponse>.CreateFailed(errorResult.Description);
        }

        await using var responseStream = await httpResponseMessage.Content.ReadAsStreamAsync();
        var result = JsonSerializer.Deserialize<OpenExchangeResponse>(responseStream, _jsonSerializerOptions);

        return result is not null
            ? Result<OpenExchangeResponse>.CreateSuccess(result)
            : Result<OpenExchangeResponse>.CreateFailed(ErrorModel.DataNotFound);
    }
}
