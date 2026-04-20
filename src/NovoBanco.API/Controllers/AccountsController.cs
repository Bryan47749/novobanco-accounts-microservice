using Microsoft.AspNetCore.Mvc;
using NovoBanco.Application.UseCases.Deposit;

namespace NovoBanco.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly CreateAccountHandler _createAccountHandler;
    private readonly GetBalanceHandler _getBalanceHandler;

    public AccountsController(CreateAccountHandler createAccountHandler, GetBalanceHandler getBalanceHandler )
    {
        _createAccountHandler = createAccountHandler;
        _getBalanceHandler = getBalanceHandler;
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


}
