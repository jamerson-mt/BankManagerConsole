using JJBanking.Domain.DTOs;
using JJBanking.Domain.Entities;
using JJBanking.Infra.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JJBanking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Contas")]
public class AccountsController : ControllerBase
{
    private readonly BankDbContext _context;

    public AccountsController(BankDbContext context)
    {
        _context = context;
    }

    // 🆔 GET: api/accounts/{id}
    // BUSCA UMA CONTA ESPECIFICA
    /// <summary>
    /// Busca os detalhes de uma conta bancária pelo seu ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var account = await _context
                .Accounts.Include(a => a.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (account == null)
                return NotFound("Conta não encontrada.");

            var response = new AccountResponse(
                account.Id,
                account.User.FullName,
                account.User.Cpf,
                account.Balance,
                account.AccountNumber,
                account.User.Cpf
            );

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // 🆔 GET: api/accounts/
    // BUSCA todas as contas
    /// <summary>
    /// Busca as contas existentes.
    /// </summary>
    /// <returns>accounts</returns>
    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        // O .Include(a => a.User) faz o "JOIN" com a tabela de usuários
        var accounts = await _context
            .Accounts.AsNoTracking()
            .Select(a => new AccountResponse(
                a.Id,
                a.User.FullName,
                a.User.Cpf,
                a.Balance,
                a.AccountNumber,
                a.Branch
            ))
            .ToListAsync();
        if (accounts.Count == 0)
            return NotFound("Sem registro no momento.");

        return Ok(accounts);
    }
}
