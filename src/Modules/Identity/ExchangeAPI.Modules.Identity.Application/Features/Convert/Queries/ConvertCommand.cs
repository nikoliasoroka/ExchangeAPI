using AutoMapper;
using ExchangeAPI.Modules.Identity.Application.Common.Interfaces.Services;
using ExchangeAPI.Shared.Common.Models.Result.Abstractions.Generics;
using ExchangeAPI.Shared.Common.Models.Result.Implementations.Generics;
using MediatR;
using System.ComponentModel.DataAnnotations;
using ExchangeAPI.Shared.Common.Constants;
using System.ComponentModel;

namespace ExchangeAPI.Modules.Identity.Application.Features.Convert.Queries;

public class ConvertCommand : IRequest<IResult<ConvertResponse>>
{
    [Required]
    [DefaultValue("EUR")]
    public string From { get; set; }
    [Required]
    [DefaultValue("USD")]
    public string To { get; set; }
    [Required]
    [DefaultValue(55.45)]
    public decimal Value { get; set; }
}

public class ConvertCommandHandler : IRequestHandler<ConvertCommand, IResult<ConvertResponse>>
{
    private readonly IOpenExchangeRatesService _exchangeRatesService;
    private readonly IMapper _mapper;

    public ConvertCommandHandler(
        IOpenExchangeRatesService exchangeRatesService,
        IMapper mapper)
    {
        _exchangeRatesService = exchangeRatesService;
        _mapper = mapper;
    }


    public async Task<IResult<ConvertResponse>> Handle(ConvertCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _exchangeRatesService.ConvertAsync(command.Value, command.From, command.To);

            if (!result.Success)
                return Result<ConvertResponse>.CreateFailed(ErrorModel.DataNotFound).AddError(result.ErrorInfo.Error);

            var response = _mapper.Map<ConvertResponse>(command);
            response.Converted = result.Data.Response;

            return Result<ConvertResponse>.CreateSuccess(response);
        }
        catch (Exception e)
        {
            return Result<ConvertResponse>.CreateFailed(e.Message);
        }
    }
}