using AutoMapper;
using ExchangeAPI.Modules.Identity.Application.Features.Convert.Queries;

namespace ExchangeAPI.Modules.Identity.Infrastructure.Mapping;

internal class OpenExchangeRateProfile : Profile
{
    public OpenExchangeRateProfile()
    {
        CreateMap<ConvertCommand, ConvertResponse>().ReverseMap();
    }
}