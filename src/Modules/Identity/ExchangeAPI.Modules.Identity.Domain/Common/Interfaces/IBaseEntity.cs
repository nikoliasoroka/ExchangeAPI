namespace ExchangeAPI.Modules.Identity.Domain.Common.Interfaces;

public interface IBaseEntity<T>
{
    public T Id { get; set; }
}