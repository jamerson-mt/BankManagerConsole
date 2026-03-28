using System.Reflection;
using JJBanking.Domain.DTOs;
using JJBanking.Domain.Entities;
using JJBanking.Domain.Interfaces;
using JJBanking.Infra.Context;
using JJBanking.Infra.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- BANCO DE DADOS (Ajustado para SQLite) ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// IMPORTANTE: Trocar .UseNpgsql por .UseSqlite
builder.Services.AddDbContext<BankDbContext>(options => options.UseSqlite(connectionString));

// --- IDENTITY (Permanece igual) ---
builder
    .Services.AddIdentity<User, IdentityRole<Guid>>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<BankDbContext>()
    .AddDefaultTokenProviders();

// --- CONFIGURAÇÃO DE CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowLocalhost",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    );
});

// --- SERVIÇOS ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{ /* ... sua config de swagger ... */
});

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// --- MIGRATIONS E CRIAÇÃO DE DIRETÓRIO ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Trecho extra para garantir que a pasta /app/data exista antes de criar o banco
        var connString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (!string.IsNullOrEmpty(connString) && connString.Contains("Data Source="))
        {
            var dbPath = connString.Replace("Data Source=", "");
            var directory = Path.GetDirectoryName(dbPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        var context = services.GetRequiredService<BankDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro ao aplicar as migrations.");
    }
}

// --- PIPELINE ---
app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection(); // Dica: Em VPS com Reverse Proxy (Nginx), às vezes é melhor desativar isso se o Nginx já cuida do SSL.
app.UseCors("AllowLocalhost");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
