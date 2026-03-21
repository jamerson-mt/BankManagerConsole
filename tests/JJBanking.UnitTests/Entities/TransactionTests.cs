using FluentAssertions;
using JJBanking.Domain.Entities;
using JJBanking.Domain.Enums;
using Xunit;

namespace JJBanking.UnitTests.Entities;

public class TransactionTests
{
    // TESTE PARA VER SE O CONSTRUTOR DA TRANSAÇÃO FUNCIONA CORRETAMENTE
    [Fact]
    public void Constructor_WhenValidData_ShouldCreateTransaction()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var amount = 150.00m;
        var type = TransactionType.Credit;
        var description = "Depósito inicial";

        // Act
        var transaction = new Transaction(accountId, amount, type, description);

        // Assert
        transaction.Amount.Should().Be(amount);
        transaction.Type.Should().Be(type);
        transaction.Description.Should().Be(description);
        transaction.Id.Should().NotBeEmpty(); // Garante que o GUID foi gerado
        transaction.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1)); // Verifica se a data de criação é recente
    }
}
