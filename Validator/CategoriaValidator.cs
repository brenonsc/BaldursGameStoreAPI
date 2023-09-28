using BaldursGame.Model;
using FluentValidation;

namespace BaldursGame.Validator;

public class CategoriaValidator : AbstractValidator<Categoria>
{
    public CategoriaValidator()
    {
        RuleFor(c => c.Tipo)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);
    }
}