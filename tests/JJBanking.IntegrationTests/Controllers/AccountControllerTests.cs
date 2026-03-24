using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using JJBanking.Domain.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace JJBanking.IntegrationTests.Controllers;

// Program é a classe principal da sua API
public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private string GenerateRandomCpf() =>
        Random.Shared.Next(100000000, 999999999).ToString() + "00";

    private readonly HttpClient _client;

    public AccountControllerTests(WebApplicationFactory<Program> factory)
    {
        // Cria um "cliente" que sabe conversar com a sua API em memória
        _client = factory.CreateClient();
    }

    // TESTE PARA BUSCA DE UMA CONTA VALIDA
    [Fact]
    public async Task AccountSearch_WhenOneAccountValid_ShouldReturnSuccess()
    {
        // Arrange: Use o ID que você sabe que existe no seu banco de teste (InMemory ou SQLite)
        var accountId = "ab46a0e3-26c5-4787-8157-7c5266068800";

        // Act: Note que passamos apenas a string do ID na URL, não o objeto 'request' inteiro
        var response = await _client.GetAsync($"/api/accounts/{accountId}");

        // Bloco de Debug aprimorado
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception(
                $"Falha na busca. Status: {response.StatusCode}. Erro: {errorContent}"
            );
        }

        // Assert
        // 1. O status de uma busca bem-sucedida é 200 OK
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // 2. Lendo o JSON usando o molde correto (AccountResponse)
        var content = await response.Content.ReadFromJsonAsync<AccountResponse>();

        content.Should().NotBeNull();
        content!.Id.Should().Be(Guid.Parse(accountId));
        content.AccountNumber.Should().NotBeNullOrEmpty();
    }

    // TESTE PARA BUSCAR TODAS AS CONTAS
    [Fact]
    public async Task AccountsSearch_WhenVeryAccounts_ShouldReturnSuccess()
    {
        // Arrange: Use o ID que você sabe que existe no seu banco de teste (InMemory ou SQLite)

        // Act: Note que passamos apenas a string do ID na URL, não o objeto 'request' inteiro
        var response = await _client.GetAsync($"/api/accounts/");

        // Bloco de Debug aprimorado
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception(
                $"Falha na busca. Status: {response.StatusCode}. Erro: {errorContent}"
            );
        }

        // Assert
        // 1. O status de uma busca bem-sucedida é 200 OK
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // TESTE PARA BUSCA DE UMA CONTA INVALIDA
    [Fact]
    public async Task AccountSearch_WhenIdFormatIsInvalid_ShouldReturnBadRequest()
    {
        // Arrange: Uma string que NÃO é um Guid (formato errado)
        var invalidId = "id-totalmente-errado-123";

        // Act
        var response = await _client.GetAsync($"/api/accounts/{invalidId}");

        // Assert
        // O ASP.NET retorna 400 porque não consegue converter a string para Guid
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // TESTE DE BUSCA QUANDO NAO HA CONTA EXISTENTE (ID)
    [Fact]
    public async Task AccountSearch_WhenAccountDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange: Um Guid válido, mas que não existe no seu Seed/Banco
        var nonExistentId = Guid.NewGuid().ToString();

        // Act
        var response = await _client.GetAsync($"/api/accounts/{nonExistentId}");

        // Assert
        // O seu Controller retorna 'return NotFound("Conta não encontrada.")'
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var errorMsg = await response.Content.ReadAsStringAsync();
        errorMsg.Should().Contain("Conta não encontrada.");
    }
}
