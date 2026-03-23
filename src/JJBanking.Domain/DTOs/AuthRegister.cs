namespace JJBanking.Domain.DTOs;

/// <summary>
/// Modelo de dados para criação de uma nova conta no JJ Banking.
/// </summary>
public class AccountRegister
{
    /// <summary>
    /// E-mail do usuário (será usado para o login).
    /// </summary>
    /// <example>jamerson@email.com</example>
    public required string Email { get; set; }

    /// <summary>
    /// Senha de acesso (mínimo 8 caracteres, com letra e número).
    /// </summary>
    /// <example>Senha@123</example>
    public required string Password { get; set; }

    /// <summary>
    /// Nome completo do titular da conta.
    /// </summary>
    /// <example>Jamerson Silva</example>
    public required string FullName { get; set; }

    /// <summary>
    /// CPF do titular (apenas números).
    /// </summary>
    /// <example>12345678901</example>
    public required string Cpf { get; set; }
}
