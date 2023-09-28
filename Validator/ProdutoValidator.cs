using BaldursGame.Model;
using FluentValidation;

namespace BaldursGame.Validator;

public class ProdutoValidator : AbstractValidator<Produto>
{
    public ProdutoValidator()
    {
        RuleFor(p => p.Titulo)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);

        RuleFor(p => p.Descricao)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(300);

        RuleFor(p => p.Console)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);

        RuleFor(p => p.DataLancamento)
            .NotEmpty();
        
        RuleFor(p => p.Preco)
            .NotEmpty();

        RuleFor(p => p.Imagem)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(200);
    }
}