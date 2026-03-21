namespace JJBanking.API.DTOs;

public record AccountResponse(Guid Id, string Owner, string Cpf, decimal Balance);
