using FluentValidation;
using MediaShop.Business.Models;
using MediaShop.Data.Interfaces;

namespace MediaShop.Business.Validation;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserDtoValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;



        RuleFor(x => x.About)
            .MaximumLength(500)
            .WithMessage("About must be less than 500 characters");

        RuleFor(x => x.VisibleName)
            .NotEmpty()
            .WithMessage("Visible name must not be empty")
            .MaximumLength(50)
            .WithMessage("Visible name must be less than 50 characters");

    }
}
