
using MoneyBankService.Api.Dto; // Necesitarás este using para AccountDto y TransactionDto
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyBankService.Application.Interfaces;

public interface IAccountService
{
    Task<IEnumerable<AccountDto>> GetAllAccountsAsync();
    Task<IEnumerable<AccountDto>> GetAccountsByAccountNumberAsync(string accountNumber);
    Task<AccountDto?> GetAccountByIdAsync(int id); // Puede devolver null si no se encuentra
    Task<AccountDto> CreateAccountAsync(AccountDto accountDto);
    Task<bool> UpdateAccountAsync(int id, AccountDto accountDto); // Devuelve bool para indicar éxito
    Task<bool> DeleteAccountAsync(int id); // Devuelve bool para indicar éxito

    // Métodos para transacciones que añadiremos más adelante:
    // Task<bool> DepositAsync(int accountId, TransactionDto transactionDto);
    // Task<bool> WithdrawAsync(int accountId, TransactionDto transactionDto);
}