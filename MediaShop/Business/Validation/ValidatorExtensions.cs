using FluentValidation;

namespace MediaShop.Business.Validation;

public static class ValidatorExtensions
{
    public static async Task<bool> ValidateAndThrowAsync<T>(this AbstractValidator<T> validator, T instance)
    {
        var result = await validator.ValidateAsync(instance);
        if (!result.IsValid)
        {
            throw new MediaShop.Exceptions.ValidationException(result.Errors.First().ErrorMessage);
        }
        return true;
    }
}
