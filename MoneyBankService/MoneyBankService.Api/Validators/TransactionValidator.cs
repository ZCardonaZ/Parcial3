using FluentValidation;
using MoneyBankService.Application.Dto; // Asegúrate que este using es correcto

namespace MoneyBankService.Api.Validators
{
    public class TransactionValidator : AbstractValidator<TransactionDto>
    {
        public TransactionValidator()
        {
            RuleFor(transaction => transaction.AccountNumber)
                .NotEmpty().WithMessage("El campo Numero de la Cuenta es Requerido.")
                .Length(10).WithMessage("El campo Numero de La Cuenta debe tener exactamente 10 caracteres.")
                .Matches(@"^\d{10}$").WithMessage("El Campo Numero de la Cuenta Solo Acepta Numeros.");

            RuleFor(transaction => transaction.ValueAmount)
                .NotEmpty().WithMessage("El campo Valor es Requerido.")
                .GreaterThan(0M).WithMessage("El campo Valor debe ser mayor a cero.")
                .PrecisionScale(18, 2, false).WithMessage("El campo Valor debe tener una precisión total de 18 dígitos y 2 decimales."); // <-- CAMBIO AQUÍ
        }
    }
}