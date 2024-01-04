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
public class ContactController : ControllerBase
{

    private readonly IMediator mediator;
    public ContactController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<ContactResponse>>> Get()
    {
        var operation = new GetAllContactQuery();
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<ContactResponse>> Get(int id)
    {
        var operation = new GetContactByIdQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }

    
    [HttpGet("GetContactByParameterQuery")]
    public async Task<ApiResponse<List<ContactResponse>>> GetContactByParameterQuery(string ContactType,string Information)
    {
        var operation = new  GetContactByParameterQuery(ContactType,Information);
        var result = await mediator.Send(operation);
        return result;
    }



    [HttpPost]
    public async Task<ApiResponse<ContactResponse>> Post([FromBody] ContactRequest Contact)
    {
        var operation = new CreateContactCommand(Contact);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse> Put(int id, [FromBody] ContactRequest Contact)
    {
        var operation = new UpdateContactCommand(id,Contact);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new DeleteContactCommand(id);
        var result = await mediator.Send(operation);
        return result;
    }
}