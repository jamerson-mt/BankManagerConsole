namespace JJBanking.API.DTOs.Auth;

// DTO de Resposta (Saída)
public record UserResponseDto(string Token, string AccountNumber, string UserName);
