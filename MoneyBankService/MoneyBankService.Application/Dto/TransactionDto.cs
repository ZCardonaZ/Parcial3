using System.ComponentModel.DataAnnotations;

namespace MoneyBankService.Application.Dto;

public class TransactionDto
{
    

    public int Id { get; set; } // Se refiere al Id de la Cuenta según el ejemplo de request.

    [Required(ErrorMessage = "El campo Numero de la Cuenta es Requerido")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "El campo Numero de La Cuenta debe tener 10 caracteres")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "El Campo Numero de la Cuenta Solo Acepta Numeros")]
    public string AccountNumber { get; set; } = null!;

    [Required(ErrorMessage = "El campo Valor es Requerido")]
    [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "El campo Valor debe ser mayor a cero.")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El campo Valor debe ser en formato Moneda (ej. 100.50)")]
    public decimal ValueAmount { get; set; }
}