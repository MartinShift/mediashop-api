using FluentValidation;
using MediaShop.Business.Models;
using MediaShop.Data.Interfaces;

namespace MediaShop.Business.Validation;

public class ProductDtoValidator : AbstractValidator<ProductDto>
{
    public ProductDtoValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Назва є обов'язковою")
            .MaximumLength(100)
            .WithMessage("Назва не повинна перевищувати 100 символів");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Опис є обов'язковим")
            .MaximumLength(500)
            .WithMessage("Опис не повинен перевищувати 500 символів");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Ціна повинна бути більше 0");

        RuleFor(x => x.UserId)
            .MustAsync(async (userId, cancellationToken) =>
            {
                var user = await unitOfWork.UserRepository.GetByIdAsync((int)userId);
                return user != null;
            })
            .WithMessage("Користувача не знайдено");
    }
}