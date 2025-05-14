// MoneyBankService.Api/Controllers/AccountsController.cs
using Microsoft.AspNetCore.Mvc;
using MoneyBankService.Application.Interfaces; // Para IAccountService
using MoneyBankService.Api.Dto; // Para AccountDto
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyBankService.Domain.Exceptions; // Para el manejo de excepciones si el servicio las lanza directamente
                                          // Aunque el ExceptionMiddleware se encargará de las no capturadas.

namespace MoneyBankService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: api/Accounts
        // GET: api/Accounts?AccountNumber=1234567890
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AccountDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Aunque el servicio podría devolver lista vacía en lugar de 404
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts([FromQuery] string? accountNumber = null)
        {
            if (!string.IsNullOrEmpty(accountNumber))
            {
                var accountsByNumber = await _accountService.GetAccountsByAccountNumberAsync(accountNumber);
                // Si no se encuentran cuentas, GetAccountsByAccountNumberAsync devuelve una lista vacía, lo cual es un 200 OK con 0 elementos.
                return Ok(accountsByNumber);
            }

            var accounts = await _accountService.GetAllAccountsAsync();
            return Ok(accounts);
        }

        // GET: api/Accounts/5
        [HttpGet("{id:int}")] // Especificamos que id debe ser int
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AccountDto>> GetAccount(int id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound($"Cuenta con Id={id} no encontrada."); // El ExceptionMiddleware puede formatear esto
                                                                       // o podrías lanzar new NotFoundException() y dejar que el middleware lo maneje.
                                                                       // Por consistencia, el middleware es mejor.
                                                                       // Lanza la excepción para que el middleware la capture:
                                                                       // throw new NotFoundException($"Cuenta con Id={id} no encontrada.");
                                                                       // Por ahora, retornaremos NotFound() directamente.
            }
            return Ok(account);
        }

        // POST: api/Accounts
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AccountDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AccountDto>> PostAccount(AccountDto accountDto)
        {
            // FluentValidation se encarga de las validaciones a nivel de modelo.
            // Las reglas de negocio (como Balance > 0) las maneja el servicio.
            // El ExceptionMiddleware se encargará de capturar BadRequestException desde el servicio.
            var createdAccount = await _accountService.CreateAccountAsync(accountDto);

            // Retorna 201 Created con la ubicación del nuevo recurso y el recurso mismo.
            return CreatedAtAction(nameof(GetAccount), new { id = createdAccount.Id }, createdAccount);
        }

        // PUT: api/Accounts/5
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // Éxito sin contenido devuelto
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAccount(int id, AccountDto accountDto)
        {
            // La validación de que id en la ruta y accountDto.Id coincidan la hace el servicio.
            // FluentValidation valida el DTO.
            // El servicio lanza NotFoundException o BadRequestException.

            // if (id != accountDto.Id) // Esta validación ahora está en el servicio
            // {
            //     return BadRequest("El Id de la ruta no coincide con el Id del cuerpo de la solicitud.");
            // }

            var success = await _accountService.UpdateAccountAsync(id, accountDto);
            // UpdateAccountAsync lanza excepciones si algo va mal (NotFound, BadRequest).
            // Si llega aquí, fue exitoso.

            // El Readme.md dice "Sin contenido" para PUT exitoso
            return NoContent();
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // Éxito sin contenido devuelto
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            // El servicio lanza NotFoundException si la cuenta no existe.
            await _accountService.DeleteAccountAsync(id);

            // El Readme.md dice "Sin contenido" para DELETE exitoso
            return NoContent();
        }

        // Los endpoints de Deposit y Withdrawal los añadiremos después.
    }
}