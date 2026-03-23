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
        var account = await _context.Accounts.FindAsync(id);

        if (account == null)
            return NotFound("Conta não encontrada.");

        var response = new AccountResponse(
            account.Id,
            account.User.FullName,
            account.User.Cpf,
            account.Balance
        );

        return Ok(response);
    }
}

// DTO (Data Transfer Object) para não expor a entidade pura no request
