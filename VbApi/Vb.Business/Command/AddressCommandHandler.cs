using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Command;

public class AddressCommandHandler :
    IRequestHandler<CreateAddressCommand, ApiResponse<AddressResponse>>,
    IRequestHandler<UpdateAddressCommand,ApiResponse>,
    IRequestHandler<DeleteAddressCommand,ApiResponse>

{
    private readonly VbDbContext dbContext;
    private readonly IMapper mapper;

    public AddressCommandHandler(VbDbContext dbContext,IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
    // Adres oluştur
    public async Task<ApiResponse<AddressResponse>> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        // Unique kontrol yapMIyorum 

        var entity = mapper.Map<AddressRequest, Address>(request.Model);

        
        var entityResult = await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var mapped = mapper.Map<Address, AddressResponse>(entityResult.Entity);
        return new ApiResponse<AddressResponse>(mapped);
    }
    // Adresler arasından ıd bilmeden [json ignore] nasıl bulup yapacaz 
    // customer ile adres arasında 1:N ilişki var customer üzerinden arayamam 
    // Şimdilik Customer Id üzerinden arıyorum ilk bulduğu adresi değiştircel
    // [JsonIgnore] u değiştirdikten sonra düzelebilir.
    public async Task<ApiResponse> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var fromdb = await dbContext.Set<Address>().Where(x => x.CustomerId == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (fromdb == null)
        {
            return new ApiResponse("Record not found");
        }
        
        fromdb.Address1 = request.Model.Address1;
        fromdb.Address2 = request.Model.Address2;
        fromdb.Country = request.Model.Country;
        fromdb.City = request.Model.City;
        fromdb.County = request.Model.County;
        fromdb.PostalCode = request.Model.PostalCode;
        
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        var fromdb = await dbContext.Set<Address>().Where(x => x.CustomerId == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (fromdb == null)
        {
            return new ApiResponse("Record not found");
        }
        //dbContext.Set<Address>().Remove(fromdb);
        
        fromdb.IsActive = false;
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}