using FluentAssertions;
using NovoBanco.Application.UseCases.Withdraw;
using NovoBanco.Domain.Entities;
using NovoBanco.Domain.Enums;
using Xunit;

public class WithdrawHandlerTests
{
    [Fact]
    public async Task Should_Withdraw_Successfully()
    {
        var context = TestDbContextFactory.Create();

        var account = new Account
        {
            Id = Guid.NewGuid(),
            Balance = 100,
            AccountNumber = $"ACC-{Guid.NewGuid().ToString().Substring(0, 8)}",
            Status = AccountStatus.ACTIVE
        };

        context.Accounts.Add(account);
        await context.SaveChangesAsync(CancellationToken.None);

        var handler = new WithdrawHandler(context);

        var request = new WithdrawRequest
        {
            AccountId = account.Id,
            Amount = 50,
            Reference = "W-001"
        };

        await handler.Handle(request, CancellationToken.None);

        account.Balance.Should().Be(50);
    }

    [Fact]
    public async Task Should_Fail_When_Insufficient_Balance()
    {
        var context = TestDbContextFactory.Create();

        var account = new Account
        {
            Id = Guid.NewGuid(),
            Balance = 10,
            AccountNumber = $"ACC-{Guid.NewGuid().ToString().Substring(0, 8)}",
            Status = AccountStatus.ACTIVE
        };

        context.Accounts.Add(account);
        await context.SaveChangesAsync(CancellationToken.None);

        var handler = new WithdrawHandler(context);

        var request = new WithdrawRequest
        {
            AccountId = account.Id,
            Amount = 50,
            Reference = "W-002"
        };

        await Assert.ThrowsAsync<Exception>(() =>
            handler.Handle(request, CancellationToken.None));
    }
}
