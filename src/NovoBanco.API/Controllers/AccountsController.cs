using Microsoft.AspNetCore.Mvc;
using NovoBanco.Application.UseCases.Deposit;

namespace NovoBanco.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly CreateAccountHandler _createAccountHandler;

    public AccountsController(CreateAccountHandler createAccountHandler)
    {
        _createAccountHandler = createAccountHandler;
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

}
