using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace VbApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EftTransactionController : ControllerBase
{

    private readonly IMediator mediator;
    public EftTransactionController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<EftTransactionResponse>>> Get()
    {
        var operation = new GetAllEftTransactionQuery();
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<EftTransactionResponse>> Get(int id)
    {
        var operation = new GetEftTransactionByIdQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }

    // daha güzel bi kullanım ayarlanabilir
    [HttpGet("GetEftTransactionByParameterQuery")]
    public async Task<ApiResponse<List<EftTransactionResponse>>> GetEftTransactionByParameterQuery(DateTime TransactionDate, string SenderIban)
    {
        var operation = new GetEftTransactionByParameterQuery(TransactionDate,SenderIban);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPost]
    public async Task<ApiResponse<EftTransactionResponse>> Post([FromBody] EftTransactionRequest EftTransaction)
    {
        var operation = new CreateEftTransactionCommand(EftTransaction);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse> Put(int id, [FromBody] EftTransactionRequest EftTransaction)
    {
        var operation = new UpdateEftTransactionCommand(id, EftTransaction);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new DeleteEftTransactionCommand(id);
        var result = await mediator.Send(operation);
        return result;
    }


}