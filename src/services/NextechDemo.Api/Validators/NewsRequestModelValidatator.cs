using FluentValidation;
using NextechDemo.Shared.Models;
using System.Diagnostics.CodeAnalysis;

namespace NextechDemo.Api.Validators
{
    [ExcludeFromCodeCoverage]
    public class NewsRequestModelValidatator : AbstractValidator<PageParams>
    {
        public NewsRequestModelValidatator()
        {
            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("Invalid page size");
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Invalid page number");

            RuleFor(x => x.IsFullDataRequired)
           .NotNull()
           .WithMessage("IsFullDataRequired not valid");
        }
    }
}
