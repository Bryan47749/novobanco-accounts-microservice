using Microsoft.AspNetCore.Mvc;
using NovoBanco.Application.UseCases.Deposit;

namespace NovoBanco.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly CreateAccountHandler _createAccountHandler;
    private readonly GetBalanceHandler _getBalanceHandler;
    private readonly GetTransactionsHandler _getTransactionsHandler;

    public AccountsController(CreateAccountHandler createAccountHandler, GetBalanceHandler getBalanceHandler, GetTransactionsHandler getTransactionsHandler)
    {
        _createAccountHandler = createAccountHandler;
        _getBalanceHandler = getBalanceHandler;
        _getTransactionsHandler = getTransactionsHandler;
    }

    [HttpPost("account")]
    public async Task<IActionResult> CreateAccount(
    CreateAccountRequest request,
    CancellationToken cancellationToken)
    {
        var accountId = await _createAccountHandler.Handle(request, cancellationToken);

        return Ok(new
        {
            accountId
        });
    }

    [HttpGet("account/{accountNumber}/balance")]
    public async Task<IActionResult> GetBalance(string accountNumber, CancellationToken cancellationToken)
    {
        var result = await _getBalanceHandler.Handle(accountNumber, cancellationToken);
        return Ok(result);
    }

    [HttpGet("account/{id}/transactions")]
    public async Task<IActionResult> GetTransactions(
    Guid id,
    int page = 1,
    int pageSize = 20,
    CancellationToken cancellationToken = default)
    {
        var result = await _getTransactionsHandler.Handle(id, page, pageSize, cancellationToken);
        return Ok(result);
    }

}
