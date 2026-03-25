using FluentValidation;
using Inventory.Api.DTOs;

namespace Inventory.Api.Validators;

/// <summary>
/// FluentValidation validator за CreateProductDto.
/// </summary>
public class CreateProductValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Името на продукта е задължително.")
            .MaximumLength(100).WithMessage("Името на продукта не може да надвишава 100 символа.")
            .MinimumLength(2).WithMessage("Името на продукта трябва да бъде поне 2 символа.");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Количеството трябва да бъде 0 или положително число.")
            .LessThanOrEqualTo(1000000).WithMessage("Количеството не може да надвишава 1,000,000.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Цената трябва да бъде положително число.")
            .LessThanOrEqualTo(999999.99m).WithMessage("Цената не може да надвишава 999,999.99.");

        RuleFor(x => x.Supplier)
            .NotEmpty().WithMessage("Доставчикът е задължителен.")
            .MaximumLength(100).WithMessage("Името на доставчика не може да надвишава 100 символа.")
            .MinimumLength(2).WithMessage("Името на доставчика трябва да бъде поне 2 символа.");
    }
}
