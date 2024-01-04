using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Schema;

namespace VbApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AddressesController : ControllerBase
{
       private readonly IMediator mediator;
    public AddressesController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<AddressResponse>>> Get()
    {
        var operation = new GetAllAddressQuery();
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<AddressResponse>> Get(int id)
    {
        var operation = new GetAddressByIdQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }

        
    [HttpGet("GetAddressByParameterQuery")]
    public async Task<ApiResponse<List<AddressResponse>>> GetAddressByParameterQuery(string Address1,string Address2,string Country,string City,string County, string PostalCode)
    {
        var operation = new  GetAddressByParameterQuery(Address1,Address2,Country,City,County,PostalCode);
        var result = await mediator.Send(operation);
        return result;
    }



    [HttpPost]
    public async Task<ApiResponse<AddressResponse>> Post([FromBody] AddressRequest Addresses)
    {
        var operation = new CreateAddressCommand(Addresses);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse> Put(int id, [FromBody] AddressRequest Addresses)
    {
        var operation = new UpdateAddressCommand(id,Addresses);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new DeleteAddressCommand(id);
        var result = await mediator.Send(operation);
        return result;
    }
    
}