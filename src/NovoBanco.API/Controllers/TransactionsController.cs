using Microsoft.AspNetCore.Mvc;
using NovoBanco.Application.UseCases.Deposit;
using NovoBanco.Application.UseCases.Transfer;
using NovoBanco.Application.UseCases.Withdraw;

namespace NovoBanco.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly DepositHandler _depositHandler;
    private readonly WithdrawHandler _withdrawHandler;
    private readonly TransferHandler _transferHandler;

    public TransactionsController(DepositHandler depositHandler, WithdrawHandler withdrawHandler, TransferHandler transferHandler)
    {
        _depositHandler = depositHandler;
        _withdrawHandler = withdrawHandler;
        _transferHandler = transferHandler;
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

    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw(
        [FromBody] WithdrawRequest request,
        CancellationToken cancellationToken)
    {
        await _withdrawHandler.Handle(request, cancellationToken);

        return Ok(new
        {
            message = "Withdraw successful"
        });
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer(
        [FromBody] TransferRequest request,
        CancellationToken cancellationToken)
    {
        await _transferHandler.Handle(request, cancellationToken);

        return Ok(new
        {
            message = "Transfer successful"
        });
    }
    
}
