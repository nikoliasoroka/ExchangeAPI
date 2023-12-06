using ExchangeAPI.Modules.Identity.Application.Features.Convert.Queries;
using FluentValidation;

namespace ExchangeAPI.Modules.Identity.Application.Features.Convert.Validators;

public class ConvertCommandValidator : AbstractValidator<ConvertCommand>
{
    public ConvertCommandValidator()
    {
        RuleFor(request => request.Value)
            .NotNull();

        RuleFor(request => request.From)
            .NotNull().NotEmpty();

        RuleFor(request => request.To)
            .NotNull().NotEmpty();
    }
}