using FluentAssertions;
using NovoBanco.Application.UseCases.Deposit;
using NovoBanco.Domain.Entities;
using NovoBanco.Domain.Enums;
using Xunit;

public class DepositHandlerTests
{
    [Fact]
    public async Task Should_Deposit_Successfully()
    {
        var context = TestDbContextFactory.Create();

        var account = new Account
        {
            Id = Guid.NewGuid(),
            AccountNumber = $"ACC-{Guid.NewGuid().ToString().Substring(0, 8)}", // ← asignar valor
            Balance = 100,
            Status = AccountStatus.ACTIVE
        };


        context.Accounts.Add(account);
        await context.SaveChangesAsync(CancellationToken.None);

        var handler = new DepositHandler(context);

        var request = new DepositRequest
        {
            AccountId = account.Id,
            Amount = 50,
            Reference = "DEP-001"
        };

        await handler.Handle(request, CancellationToken.None);

        account.Balance.Should().Be(150);
    }

    [Fact]
    public async Task Should_Throw_When_Account_Not_Found()
    {
        var context = TestDbContextFactory.Create();
        var handler = new DepositHandler(context);

        var request = new DepositRequest
        {
            AccountId = Guid.NewGuid(),
            Amount = 50,
            Reference = "DEP-002"
        };

        await Assert.ThrowsAsync<Exception>(() =>
            handler.Handle(request, CancellationToken.None));
    }
}
