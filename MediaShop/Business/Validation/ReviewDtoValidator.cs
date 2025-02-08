using FluentValidation;
using MediaShop.Business.Models;
using MediaShop.Data.Interfaces;

namespace MediaShop.Business.Validation;

public class ReviewDtoValidator : AbstractValidator<ReviewDto>
{
    private IUnitOfWork _unitOfWork { get; }
    public ReviewDtoValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        RuleFor(x => x.Text)
            .NotEmpty()
            .WithMessage("Review text is required")
            .MaximumLength(300)
            .WithMessage("Review text is too long");

        RuleFor(x=> x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5");

        RuleFor(x => x)
            .MustAsync(async (review, token) =>
            {
                var entity = await _unitOfWork.ReviewRepository.GetByUserIdAsync(review.UserId);
                return !entity.Any(x => x.ProductId == review.ProductId);
            })
            .WithMessage("Review for this product already exists");
    }
}
