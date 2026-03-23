namespace JJBanking.API.DTOs;

public record CreatedAccountRequest(string Owner, string Cpf, decimal InitialDeposit);
