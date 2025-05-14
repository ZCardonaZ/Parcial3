using FluentValidation;
using MoneyBankService.Api.Dto;

namespace MoneyBankService.Api.Validators;

public class TransactionValidator : AbstractValidator<TransactionDto>
{
    public TransactionValidator()
    {
        // El Id de la cuenta no se valida aquí, se asume que es válido si llega a este punto
        // o se valida por la ruta del endpoint.

        RuleFor(transaction => transaction.AccountNumber)
            .NotEmpty().WithMessage("El campo Numero de la Cuenta es Requerido.")
            .Length(10).WithMessage("El campo Numero de La Cuenta debe tener exactamente 10 caracteres.")
            .Matches(@"^\d{10}$").WithMessage("El Campo Numero de la Cuenta Solo Acepta Numeros.");

        RuleFor(transaction => transaction.ValueAmount)
            .NotEmpty().WithMessage("El campo Valor es Requerido.")
            .GreaterThan(0M).WithMessage("El campo Valor debe ser mayor a cero.")
            .Matches(@"^\d+(\.\d{1,2})?$").WithMessage("El campo Valor debe ser en formato Moneda (ej. 100.50).");
    }
}