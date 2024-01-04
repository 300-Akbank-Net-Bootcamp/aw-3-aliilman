using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Command;

public class AccountCommandHandler :
    IRequestHandler<CreateAccountCommand, ApiResponse<AccountResponse>>,
    IRequestHandler<UpdateAccountCommand, ApiResponse>,
    IRequestHandler<DeleteAccountCommand, ApiResponse>

{
    private readonly VbDbContext dbContext;
    private readonly IMapper mapper;

    public AccountCommandHandler(VbDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    // Account oluşturma
    public async Task<ApiResponse<AccountResponse>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        // Hesap daha önce var mı kontorl et
        var checkIdentity = await dbContext.Set<Account>().Where(x => x.AccountNumber == request.Model.AccountNumber)
    .FirstOrDefaultAsync(cancellationToken);
        if (checkIdentity != null)
        {
            return new ApiResponse<AccountResponse>($"{request.Model.AccountNumber} is used by another customer.");
        }

        // Acconut bilgilerini db uygun hale maping 
        var entity = mapper.Map<AccountRequest, Account>(request.Model);

        //Şimdilik random olarak alır. 
        // unique kontrolu yaılabilir.
        entity.AccountNumber = new Random().Next(1000000, 9999999);

        var entityResult = await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var mapped = mapper.Map<Account, AccountResponse>(entityResult.Entity);
        return new ApiResponse<AccountResponse>(mapped);
    }

    //update 
    public async Task<ApiResponse> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        // hesapı bul
        var fromdb = await dbContext.Set<Account>().Where(x => x.AccountNumber == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (fromdb == null)
        {
            return new ApiResponse("Record not found");
        }

        fromdb.Name = request.Model.Name;
        fromdb.Name = request.Model.Name;

        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        // Hesabı bul 
        var fromdb = await dbContext.Set<Account>().Where(x => x.AccountNumber == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (fromdb == null)
        {
            return new ApiResponse("Record not found");
        }
        //dbContext.Set<Account>().Remove(fromdb);

        //dbContext.Remove(await dbContext.Set<Account>().Where(x => x.AccountNumber == accountNumber).FirstOrDefaultAsync());
        fromdb.IsActive = false;
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse();
    }
}