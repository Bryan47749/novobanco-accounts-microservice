using Microsoft.AspNetCore.Mvc;
using NovoBanco.Application.UseCases.Deposit;

namespace NovoBanco.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly DepositHandler _depositHandler;

    public TransactionsController(DepositHandler depositHandler)
    {
        _depositHandler = depositHandler;
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit(
        [FromBody] DepositRequest request,
        CancellationToken cancellationToken)
    {
        await _depositHandler.Handle(request, cancellationToken);

        return Ok(new
        {
            message = "Deposit successful"
        });
    }
}
