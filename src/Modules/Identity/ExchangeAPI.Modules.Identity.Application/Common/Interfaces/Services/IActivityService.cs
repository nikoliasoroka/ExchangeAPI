using ExchangeAPI.Modules.Identity.Domain.Entities;

namespace ExchangeAPI.Modules.Identity.Application.Common.Interfaces.Services;

public interface IActivityService
{
    IQueryable<Activity> GetActivities();
    Task SaveActivity(string userName, string category, string result, CancellationToken cancellationToken);
}