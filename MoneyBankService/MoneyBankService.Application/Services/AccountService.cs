using AutoMapper; 
using MoneyBankService.Application.Interfaces;
using MoneyBankService.Application.Dto;
using MoneyBankService.Domain.Entities;
using MoneyBankService.Domain.Interfaces.Repositories; 
using MoneyBankService.Domain.Exceptions; 
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq; 

namespace MoneyBankService.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;
    private const decimal MAX_OVERDRAFT = 1_000_000M;  

    public AccountService(IAccountRepository accountRepository, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
    }

    public async Task<AccountDto> CreateAccountAsync(AccountDto accountDto)
    {
       
        if (accountDto.BalanceAmount <= 0)
        {
            throw new BadRequestException("El Balance debe ser mayor a cero.");
        }

        
        var existingAccountByNumber = await _accountRepository.FindAsync(acc => acc.AccountNumber == accountDto.AccountNumber);
        if (existingAccountByNumber.Any())
        {
            throw new BadRequestException($"La Cuenta [{accountDto.AccountNumber}] ya Existe.");
        }

        var account = _mapper.Map<Account>(accountDto);

       
        if (account.AccountType == 'C')
        {
            account.BalanceAmount += MAX_OVERDRAFT;
           
        }
     

        var createdAccount = await _accountRepository.AddAsync(account);
        return _mapper.Map<AccountDto>(createdAccount);
    }

    public async Task<bool> DeleteAccountAsync(int id)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        if (account == null)
        {
            throw new NotFoundException($"Cuenta con Id={id} no encontrada.");
        }
        await _accountRepository.RemoveAsync(account);
        return true; 
    }

    public async Task<AccountDto?> GetAccountByIdAsync(int id)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        if (account == null)
        {
            
            return null;
        }
        return _mapper.Map<AccountDto>(account);
    }

    public async Task<IEnumerable<AccountDto>> GetAccountsByAccountNumberAsync(string accountNumber)
    {
        var accounts = await _accountRepository.FindAsync(acc => acc.AccountNumber == accountNumber);
        return _mapper.Map<IEnumerable<AccountDto>>(accounts);
    }

    public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
    {
        var accounts = await _accountRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<AccountDto>>(accounts);
    }

    public async Task<bool> UpdateAccountAsync(int id, AccountDto accountDto)
    {
        if (id != accountDto.Id)
        {
            throw new BadRequestException("El Id de la ruta no coincide con el Id del cuerpo de la solicitud.");
        }

        var accountToUpdate = await _accountRepository.GetByIdAsync(id);
        if (accountToUpdate == null)
        {
            throw new NotFoundException($"Cuenta con Id={id} no encontrada para actualizar.");
        }

        
        if (accountToUpdate.AccountNumber != accountDto.AccountNumber)
        {
            var existingAccountByNewNumber = await _accountRepository.FindAsync(acc => acc.AccountNumber == accountDto.AccountNumber && acc.Id != id);
            if (existingAccountByNewNumber.Any())
            {
                throw new BadRequestException($"El nuevo número de cuenta [{accountDto.AccountNumber}] ya está en uso por otra cuenta.");
            }
        }

       
        accountToUpdate.OwnerName = accountDto.OwnerName;
        accountToUpdate.AccountType = accountDto.AccountType; 
        _mapper.Map(accountDto, accountToUpdate);


        await _accountRepository.UpdateAsync(accountToUpdate);
        return true;
    }

    // Aquí irán los métodos DepositAsync y WithdrawAsync más adelante.
}