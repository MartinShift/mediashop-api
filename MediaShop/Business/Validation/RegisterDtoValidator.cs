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
        .WithMessage("���������� ����� �� ���� ���� ���������")
        .Matches("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")
        .WithMessage("������������ ������ ���������� �����");

        RuleFor(x => x.Email)
        .MustAsync(async (email, token) =>
        {
            return await _unitOfWork.UserRepository.GetByEmailAsync(email) == null;
        })
        .WithMessage("���������� � ����� ����������� ������ ��� ����");

        RuleFor(x => x.UserName)
        .MustAsync(async (username, token) =>
        {
            return await _unitOfWork.UserRepository.GetByUsernameAsync(username) == null;
        })
        .WithMessage("���������� � ����� ������ ��� ����");

        RuleFor(x => x.Password)
           .NotEmpty().WithMessage("������ � ����'�������.")
           .Length(8, 15).WithMessage("������ ������� ���� �� 8 �� 15 �������.")
           .Matches("[a-z]").WithMessage("������ ������� ������ ���� � ���� ���� �����.")
           .Matches("[A-Z�-�]").WithMessage("������ ������� ������ ���� � ���� ������ �����.")
           .Matches("[0-9]").WithMessage("������ ������� ������ ���� � ���� �����.");

        RuleFor(r => r)
        .Must(r => r.Password == r.ConfirmPassword)
        .WithMessage("����� �� ����������");
    }
}