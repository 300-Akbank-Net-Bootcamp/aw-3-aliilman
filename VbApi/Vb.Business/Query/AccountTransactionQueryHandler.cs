using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Query;

public class AccountTransactionQueryHandler :
    IRequestHandler<GetAllAccountTransactionQuery, ApiResponse<List<AccountTransactionResponse>>>,
    IRequestHandler<GetAccountTransactionByIdQuery, ApiResponse<AccountTransactionResponse>>,
    IRequestHandler<GetAccountTransactionByParameterQuery, ApiResponse<List<AccountTransactionResponse>>>
{
    private readonly VbDbContext dbContext;
    private readonly IMapper mapper;

    public AccountTransactionQueryHandler(VbDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    // Tüm transferleri getir
    public async Task<ApiResponse<List<AccountTransactionResponse>>> Handle(GetAllAccountTransactionQuery request,
        CancellationToken cancellationToken)
    {
        var list = await dbContext.Set<AccountTransaction>()
            .Include(x => x.Account)
            // .Include(x => x.AccountTransactionTransactions) //Bunları içermese de olur
            // .Include(x => x.EftTransactions)
            .ToListAsync(cancellationToken);
        
        var mappedList = mapper.Map<List<AccountTransaction>, List<AccountTransactionResponse>>(list);
         return new ApiResponse<List<AccountTransactionResponse>>(mappedList);
    }

    //by id
    public async Task<ApiResponse<AccountTransactionResponse>> Handle(GetAccountTransactionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity =  await dbContext.Set<AccountTransaction>()
           .Include(x => x.Account)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse<AccountTransactionResponse>("Record not found");
        }
        
        var mapped = mapper.Map<AccountTransaction, AccountTransactionResponse>(entity);
        return new ApiResponse<AccountTransactionResponse>(mapped);
    }

    //parametre değerlerine göre sorgular 
    public async Task<ApiResponse<List<AccountTransactionResponse>>> Handle(GetAccountTransactionByParameterQuery request,
        CancellationToken cancellationToken)
    {
        var list =  await dbContext.Set<AccountTransaction>()
           .Include(x => x.Account)
            // .Include(x => x.AccountTransactionTransactions) // buna gerek olamayabilir
            // .Include(x => x.EftTransactions)
            .Where(x =>
            x.Description.ToUpper().Contains(request.Description.ToUpper()) ||
            x.TransferType.ToUpper().Contains(request.TransferType.ToUpper()) ||
            (x.TransactionDate==request.TransactionDate)
        ).ToListAsync(cancellationToken);
        
        var mappedList = mapper.Map<List<AccountTransaction>, List<AccountTransactionResponse>>(list);
        return new ApiResponse<List<AccountTransactionResponse>>(mappedList);
    }
}