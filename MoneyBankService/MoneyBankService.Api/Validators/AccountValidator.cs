using FluentValidation;
using MoneyBankService.Api.Dto;
using System; // Para DateTime

namespace MoneyBankService.Api.Validators;

public class AccountValidator : AbstractValidator<AccountDto>
{
    public AccountValidator()
    {
        RuleFor(account => account.AccountType)
            .NotEmpty().WithMessage("El campo Tipo de Cuenta es Requerido.")
            .Matches("[AC]").WithMessage("El campo Tipo de Cuenta solo permite (A o C).");

        // CreationDate usualmente no se valida en la entrada si se asigna por defecto,
        // pero si se permite enviarla, podríamos añadir:
        // RuleFor(account => account.CreationDate)
        //     .NotEmpty().WithMessage("La Fecha de Creación es requerida.")
        //     .LessThanOrEqualTo(DateTime.Now).WithMessage("La Fecha de Creación no puede ser futura.");

        RuleFor(account => account.AccountNumber)
            .NotEmpty().WithMessage("El campo Numero de la Cuenta es Requerido.")
            .Length(10).WithMessage("El campo Numero de La Cuenta debe tener exactamente 10 caracteres.")
            .Matches(@"^\d{10}$").WithMessage("El Campo Numero de la Cuenta Solo Acepta Numeros.");

        RuleFor(account => account.OwnerName)
            .NotEmpty().WithMessage("El campo Nombre del Propietario es Requerido.")
            .MaximumLength(100).WithMessage("El campo Nombre del Propietario tiene una longitud maxima de 100 caracteres.");

        RuleFor(account => account.BalanceAmount)
            .NotEmpty().WithMessage("El campo Balance es Requerido.")
            .Matches(@"^\d+(\.\d{1,2})?$").WithMessage("El campo Balance debe ser en formato Moneda (ej. 1000 o 1000.00).")
            // La regla "El Balance debe ser mayor a cero" para la *creación*
            // es una regla de negocio que a menudo se maneja en el servicio o
            // se puede añadir condicionalmente aquí si el validador se usa en diferentes contextos.
            // Por ahora, la expresión regular y NotEmpty son suficientes para el formato.
            // Podríamos añadir .GreaterThan(0) si este validador solo se usa para creación.
            // RuleFor(account => account.BalanceAmount)
            // .GreaterThan(0M).WithMessage("El Balance debe ser mayor a cero.")
            // .When(account => account.Id == 0, ApplyConditionTo.CurrentValidator); // Ejemplo si el Id diferencia creación de actualización

        RuleFor(account => account.OverdraftAmount)
            .NotEmpty().WithMessage("El campo Sobregiro es Requerido.")
            .Matches(@"^\d+(\.\d{1,2})?$").WithMessage("El campo Sobregiro debe ser en formato Moneda (ej. 1000 o 1000.00).")
            .GreaterThanOrEqualTo(0M).WithMessage("El campo Sobregiro no puede ser negativo."); // El sobregiro puede ser cero.
    }
}