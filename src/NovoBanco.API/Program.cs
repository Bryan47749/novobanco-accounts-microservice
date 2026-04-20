using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using NovoBanco.API.Middleware;
using NovoBanco.Application.Interfaces;
using NovoBanco.Application.UseCases.Deposit;
using NovoBanco.Domain.Entities;
using NovoBanco.Domain.Enums;
using NovoBanco.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NovoBancoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IApplicationDbContext, NovoBancoDbContext>();
builder.Services.AddScoped<DepositHandler>();
builder.Services.AddScoped<CreateAccountHandler>();
builder.Services.AddScoped<GetBalanceHandler>();
builder.Services.AddScoped<GetTransactionsHandler>();


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Migraciones automáticas
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NovoBancoDbContext>();
    db.Database.Migrate();
    if (!db.Customers.Any())
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Identification = "0503947749",
            FirstName = "Bryan",
            LastName = "Toalumbo",
            CreatedAt = DateTime.UtcNow
        };

        var account = new Account
        {
            Id = Guid.NewGuid(),
            CustomerId = customer.Id,
            AccountNumber = "ACC-0001",
            Balance = 1000m,
            Currency = "USD",
            Type = AccountType.SAVINGS,
            Status = AccountStatus.ACTIVE,
            CreatedAt = DateTime.UtcNow
        };

        db.Customers.Add(customer);
        db.Accounts.Add(account);
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
