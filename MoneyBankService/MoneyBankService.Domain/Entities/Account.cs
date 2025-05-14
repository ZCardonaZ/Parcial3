using MoneyBankService.Domain.Common;
using System; // Para DateTime
using System.ComponentModel.DataAnnotations; // Para las anotaciones

namespace MoneyBankService.Domain.Entities
{
    public class Account : EntityBase // EntityBase ya tiene public int Id { get; set; } con [Key]
    {
        [Required(ErrorMessage = "El campo Tipo de Cuenta es Requerido")]
        [RegularExpression("[AC]", ErrorMessage = "El campo Tipo de Cuenta solo permite (A o C)")]
        public char AccountType { get; set; } = 'A'; // Valor por defecto 'A' como en el Readme

        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; } = DateTime.Now; // Valor por defecto como en el Readme

        [Required(ErrorMessage = "El campo Numero de la Cuenta es Requerido")]
        [MaxLength(10, ErrorMessage = "El campo Numero de La Cuenta tiene una longitud maxima de 10 caracteres")]
        [RegularExpression(@"\d{10}", ErrorMessage = "El Campo Numero de la Cuenta Solo Acepta Numeros")]
        public string AccountNumber { get; set; } = null!; // null! para C# 8+ con tipos de referencia anulables

        [Required(ErrorMessage = "El campo Nombre del Propietario es Requerido")]
        [MaxLength(100, ErrorMessage = "El campo Nombre del Propietario tiene una longitud maxima de 100 caracteres")]
        public string OwnerName { get; set; } = null!;

        [Required(ErrorMessage = "El campo Balance es Requerido")]
        [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "El campo Balance debe ser en formato Moneda (0.00)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El Balance debe ser mayor a cero")] // El Readme indica "El Balance debe ser mayor a cero", esto se puede validar aquí o en la lógica de servicio/DTO.
        public decimal BalanceAmount { get; set; }

        [Required(ErrorMessage = "El campo Sobregiro es Requerido")]
        [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "El campo Sobregiro debe ser en formato Moneda (0.00)")]
        public decimal OverdraftAmount { get; set; }
    }
}