using ExchangeAPI.Modules.Identity.Application.Common.Interfaces.Context;
using ExchangeAPI.Modules.Identity.Application.Common.Interfaces.Services;
using ExchangeAPI.Modules.Identity.Domain.Entities;

namespace ExchangeAPI.Modules.Identity.Infrastructure.Services;

public class ActivityService : IActivityService
{
    private readonly IDataContext _context;

    public ActivityService(IDataContext context)
    {
        _context = context;
    }

    public IQueryable<Activity> GetActivities() => _context.Activity;

    public async Task SaveActivity(string userName, string category, string result, CancellationToken cancellationToken)
    {
        var activity = new Activity(userName, category, result);

        await _context.Activity.AddAsync(activity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}