using FluentValidation;
using MediaShop.Data.Interfaces;

namespace MediaShop.Business.Services;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public RegisterDtoValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.Email)
        .NotEmpty()
        .WithMessage("Електронна пошта не може бути порожньою")
        .Matches("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")
        .WithMessage("Неправильний формат електронної пошти");

        RuleFor(x => x.Email)
        .MustAsync(async (email, token) =>
        {
            return await _unitOfWork.UserRepository.GetByEmailAsync(email) == null;
        })
        .WithMessage("Користувач з такою електронною поштою вже існує");

        RuleFor(x => x.UserName)
        .MustAsync(async (username, token) =>
        {
            return await _unitOfWork.UserRepository.GetByUsernameAsync(username) == null;
        })
        .WithMessage("Користувач з таким логіном вже існує");

        RuleFor(x => x.Password)
           .NotEmpty().WithMessage("Пароль є обов'язковим.")
           .Length(8, 15).WithMessage("Пароль повинен бути від 8 до 15 символів.")
           .Matches("[a-z]").WithMessage("Пароль повинен містити хоча б одну малу літеру.")
           .Matches("[A-ZА-Я]").WithMessage("Пароль повинен містити хоча б одну велику літеру.")
           .Matches("[0-9]").WithMessage("Пароль повинен містити хоча б одну цифру.");

        RuleFor(r => r)
        .Must(r => r.Password == r.ConfirmPassword)
        .WithMessage("Паролі не співпадають");
    }
}