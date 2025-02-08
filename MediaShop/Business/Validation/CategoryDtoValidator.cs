using FluentValidation;
using MediaShop.Business.Models;
using MediaShop.Data.Interfaces;

namespace MediaShop.Business.Validation
{
    public class CategoryDtoValidator : AbstractValidator<CategoryDto>
    {
        private IUnitOfWork _unitOfWork { get; }
        public CategoryDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Назва категорії не має бути пустою")
                .MaximumLength(30)
                .WithMessage("Назва категорії не має містити 30 символів");

            RuleFor(x => x)
                .MustAsync(async (category, token) =>
                {
                    var entity = await _unitOfWork.CategoryRepository.GetByNameAsync(category.Name);
                    return entity == null || entity.Id == category.Id;
                })
                .WithMessage("Дана категорія вже існує");
        }
    }
}
