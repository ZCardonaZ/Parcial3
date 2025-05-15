using FluentValidation;
using MoneyBankService.Application.Dto; // Asegúrate que este using es correcto
using System;

namespace MoneyBankService.Api.Validators
{
    public class AccountValidator : AbstractValidator<AccountDto>
    {
        public AccountValidator()
        {
            RuleFor(account => account.AccountType)
                .NotEmpty().WithMessage("El campo Tipo de Cuenta es Requerido.")
                .Must(type => type == 'A' || type == 'C') // <-- CAMBIO AQUÍ
                .WithMessage("El campo Tipo de Cuenta solo permite (A o C).");

            RuleFor(account => account.AccountNumber)
                .NotEmpty().WithMessage("El campo Numero de la Cuenta es Requerido.")
                .Length(10).WithMessage("El campo Numero de La Cuenta debe tener exactamente 10 caracteres.")
                .Matches(@"^\d{10}$").WithMessage("El Campo Numero de la Cuenta Solo Acepta Numeros.");

            RuleFor(account => account.OwnerName)
                .NotEmpty().WithMessage("El campo Nombre del Propietario es Requerido.")
                .MaximumLength(100).WithMessage("El campo Nombre del Propietario tiene una longitud maxima de 100 caracteres.");

            RuleFor(account => account.BalanceAmount)
                .NotEmpty().WithMessage("El campo Balance es Requerido.")
                .PrecisionScale(18, 2, false).WithMessage("El campo Balance debe tener una precisión total de 18 dígitos y 2 decimales.") // <-- CAMBIO AQUÍ
                .GreaterThan(0M).When(acc => acc.Id == 0, ApplyConditionTo.CurrentValidator).WithMessage("El Balance inicial debe ser mayor a cero.");

            RuleFor(account => account.OverdraftAmount)
                .NotEmpty().WithMessage("El campo Sobregiro es Requerido.")
                .PrecisionScale(18, 2, false).WithMessage("El campo Sobregiro debe tener una precisión total de 18 dígitos y 2 decimales.") // <-- CAMBIO AQUÍ
                .GreaterThanOrEqualTo(0M).WithMessage("El campo Sobregiro no puede ser negativo.");
        }
    }
}