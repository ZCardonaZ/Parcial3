using System;
using System.ComponentModel.DataAnnotations;
namespace MoneyBankService.Application.Dto;

public class AccountDto
{
    public int Id { get; set; } 

    [Required(ErrorMessage = "El campo Tipo de Cuenta es Requerido")]
    [RegularExpression("[AC]", ErrorMessage = "El campo Tipo de Cuenta solo permite (A o C)")]
    public char AccountType { get; set; }

    [DataType(DataType.Date)]
    public DateTime CreationDate { get; set; }

    [Required(ErrorMessage = "El campo Numero de la Cuenta es Requerido")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "El campo Numero de La Cuenta debe tener 10 caracteres")] 
    [RegularExpression(@"^\d{10}$", ErrorMessage = "El Campo Numero de la Cuenta Solo Acepta Numeros")]
    public string AccountNumber { get; set; } = null!;

    [Required(ErrorMessage = "El campo Nombre del Propietario es Requerido")]
    [StringLength(100, ErrorMessage = "El campo Nombre del Propietario no debe exceder los 100 caracteres")]
    public string OwnerName { get; set; } = null!;

    [Required(ErrorMessage = "El campo Balance es Requerido")]
    [Range(0.0, (double)decimal.MaxValue, ErrorMessage = "El Balance debe ser un valor positivo.")] 
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El campo Balance debe ser en formato Moneda (ej. 1000.00)")]
    public decimal BalanceAmount { get; set; }

    [Required(ErrorMessage = "El campo Sobregiro es Requerido")]
    [Range(0.0, (double)decimal.MaxValue, ErrorMessage = "El Sobregiro debe ser un valor positivo.")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El campo Sobregiro debe ser en formato Moneda (ej. 1000.00)")]
    public decimal OverdraftAmount { get; set; }
}
