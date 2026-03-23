using JJBanking.Domain.DTOs;
using JJBanking.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace JJBanking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Operações Bancárias")]
public class TransactionController : ControllerBase
{
    private readonly IAccountService _accountService;

    public TransactionController(IAccountService accountService) //
    {
        _accountService = accountService;
    }

    // REALIZA UMA TRANSAÇÃO DE DEPÓSITO
    /// <summary>
    /// Realiza um depósito em uma conta bancária. O valor é adicionado ao saldo da conta e registrado como uma transação do tipo "Crédito".
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] DepositRequest request)
    {
        try
        {
            // Chamamos o serviço e damos um "await" na promessa (Task)
            var transaction = await _accountService.DepositAsync(
                request.AccountId, // O ID da conta para onde o dinheiro vai
                request.Amount,
                request.Description
            );

            var response = new TransactionResponse(
                transaction.Id,
                transaction.Amount,
                transaction.Type.ToString(), // Converte o Enum (0, 1) para texto ("Credit")
                transaction.Description,
                transaction.CreatedAt
            );

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // REALIZA UMA TRANSAÇÃO DE SAQUE
    /// <summary>
    /// Realiza um saque em uma conta bancária. O valor é subtraído do saldo da conta e registrado como uma transação do tipo "Débito". O serviço verifica se há saldo suficiente antes de processar o saque.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw([FromBody] DepositRequest request)
    {
        try
        {
            var transaction = await _accountService.WithdrawAsync(
                request.AccountId,
                request.Amount,
                request.Description
            );

            var response = new TransactionResponse(
                transaction.Id,
                transaction.Amount,
                transaction.Type.ToString(),
                transaction.Description,
                transaction.CreatedAt
            );

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // OBTÉM O EXTRATO DE TRANSAÇÕES DE UMA CONTA
    /// <summary>
    /// Realiza a busca de transations de uma conta
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    [HttpGet("statement/{accountId}")]
    public async Task<IActionResult> GetStatement(Guid accountId)
    {
        var transactions = await _accountService.GetStatementAsync(accountId);
        // Converte a lista de Entidades para uma lista de DTOs
        var response = transactions.Select(t => new TransactionResponse(
            t.Id,
            t.Amount,
            t.Type.ToString(),
            t.Description,
            t.CreatedAt
        ));
        return Ok(response);
    }
}
