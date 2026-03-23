using JJBanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JJBanking.Infra.Context;

public class BankDbContext : DbContext
{
    public BankDbContext(DbContextOptions<BankDbContext> options)
        : base(options) { }

    public DbSet<Account> Accounts => Set<Account>(); // Propriedade para acessar a tabela de contas
    public DbSet<Transaction> Transactions => Set<Transaction>(); // Propriedade para acessar a tabela de transações

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // (ACCOUNT) - Configurações para a entidade Account
        modelBuilder.Entity<Account>().HasKey(a => a.Id);

        //Cpf deve ser único para cada conta, garantindo que não haja duplicatas
        modelBuilder.Entity<Account>().HasIndex(a => a.Cpf).IsUnique();

        //garante que o campo Balance seja do tipo decimal com precisão de 8 dígitos e 2 casas decimais
        modelBuilder.Entity<Account>().Property(a => a.Balance).HasColumnType("decimal(8,2)");

        // (TRANSACTION) - Configurações para a entidade Transaction
        modelBuilder.Entity<Transaction>().HasKey(t => t.Id);
        modelBuilder.Entity<Transaction>().Property(t => t.Amount).HasColumnType("decimal(8,2)");

        // Relacionamento 1:N (Uma CONTA tem muitas TRANSAÇÕES)
        modelBuilder
            .Entity<Transaction>() // Configura a entidade Transaction
            .HasOne(t => t.Account) // Cada transação tem uma conta associada
            .WithMany(a => a.Transactions) // Uma conta pode ter muitas transações
            .HasForeignKey(t => t.AccountId); // Chave estrangeira na tabela de transações que aponta para a conta

        base.OnModelCreating(modelBuilder);
    }
}
