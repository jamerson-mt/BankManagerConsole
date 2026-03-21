namespace JJBanking.API.DTOs;

public record CreatedAccountResponse(string Owner, string Cpf, decimal InitialDeposit);
