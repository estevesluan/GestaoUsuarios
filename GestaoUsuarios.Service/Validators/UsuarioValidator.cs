using FluentValidation;
using GestaoUsuarios.Domain.Entities;
using System;

namespace GestaoUsuarios.Service.Validators
{
    public class UsuarioValidator : AbstractValidator<Usuario>
    {
        public UsuarioValidator()
        {
            RuleFor(c => c)
                    .NotNull()
                    .OnAnyFailure(x =>
                    {
                        throw new ArgumentNullException("Objeto não definido.");
                    });

            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("Não informado o Nome.")
                .NotNull().WithMessage("Não informado o Nome.");

            RuleFor(c => c.Sobrenome)
                .NotEmpty().WithMessage("Não informado o Sobrenome.")
                .NotNull().WithMessage("Não informado o Sobrenome.");

            RuleFor(c => c.NomeMae)
                .NotEmpty().WithMessage("Não informado o nome da mãe.")
                .NotNull().WithMessage("Não informado o nome da mãe.");

            RuleFor(c => c.Nascimento)
                .NotEmpty().WithMessage("Não informada a data de Nascimento.")
                .NotNull().WithMessage("Não informada a data de Nascimento.");

            RuleFor(c => c.Cpf)
                .NotEmpty().WithMessage("Não informado o Cpf.")
                .NotNull().WithMessage("Não informado o Cpf.");

            RuleFor(c => c.Telefone)
                .NotEmpty().WithMessage("Não informada a Senha.")
                .NotNull().WithMessage("Não informada a Senha.");

            RuleFor(c => c.Telefone)
                .NotEmpty().WithMessage("Não informado a Confirmação da Senha.")
                .NotNull().WithMessage("Não informado a Confirmação da Senha.");

            RuleFor(c => c.Telefone)
                .NotEmpty().WithMessage("Não informado o Telefone.")
                .NotNull().WithMessage("Não informado o Telefone.");

            RuleFor(c => c.Cep)
                .NotEmpty().WithMessage("Não informado o Cep.")
                .NotNull().WithMessage("Não informado o Cep.");
        }
    }
}
