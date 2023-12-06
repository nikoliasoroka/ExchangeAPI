using AutoMapper;
using ExchangeAPI.Modules.Identity.Application.Features.Account.Queries.Activity;
using ExchangeAPI.Modules.Identity.Domain.Entities;

namespace ExchangeAPI.Modules.Identity.Infrastructure.Mapping;

public class ActivityProfile : Profile
{
    public ActivityProfile()
    {
        CreateMap<Activity, GetActivityResponse>().ReverseMap();
    }
}