using AutoMapper;
using ExchangeAPI.Modules.Identity.Application.Common.Interfaces.Services;
using ExchangeAPI.Shared.Common.Interfaces;
using ExchangeAPI.Shared.Common.Models;
using ExchangeAPI.Shared.Common.Models.Result.Abstractions.Generics;
using ExchangeAPI.Shared.Common.Models.Result.Implementations.Generics;
using MediatR;

namespace ExchangeAPI.Modules.Identity.Application.Features.Account.Queries.Activity;

public class GetActivityQuery : IRequest<IResult<PaginatedList<GetActivityResponse>>>, IPagination, ISearchFilter
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
}

public class GetActivityQueryHandler : IRequestHandler<GetActivityQuery, IResult<PaginatedList<GetActivityResponse>>>
{
    private readonly IActivityService _activityService;
    private readonly IMapper _mapper;

    public GetActivityQueryHandler(
        IActivityService activityService,
        IMapper mapper)
    {
        _activityService = activityService;
        _mapper = mapper;
    }

    public async Task<IResult<PaginatedList<GetActivityResponse>>> Handle(GetActivityQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var activity = _activityService.GetActivities()
                .OrderByDescending(u => u.Date)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Search))
            {
                var search = request.Search.ToLower().Trim();

                activity = activity.Where(x => x.Username.ToLower().Contains(search) ||
                                               x.Category.ToLower().Contains(search) ||
                                               x.Result.ToLower().Contains(search) ||
                                               x.Date.ToString().Contains(search));
            }

            var result = await PaginatedList<GetActivityResponse>.CreateAsync(_mapper, activity, request.PageNumber, request.PageSize);

            return Result<PaginatedList<GetActivityResponse>>.CreateSuccess(result);
        }
        catch (Exception e)
        {
            return Result<PaginatedList<GetActivityResponse>>.CreateFailed(e.Message);
        }
    }
}