using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Query;

public class EftTransactionQueryHandler :
    IRequestHandler<GetAllEftTransactionQuery, ApiResponse<List<EftTransactionResponse>>>,
    IRequestHandler<GetEftTransactionByIdQuery, ApiResponse<EftTransactionResponse>>,
    IRequestHandler<GetEftTransactionByParameterQuery, ApiResponse<List<EftTransactionResponse>>>
{
    private readonly VbDbContext dbContext;
    private readonly IMapper mapper;

    public EftTransactionQueryHandler(VbDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    // Tüm transferleri getir
    public async Task<ApiResponse<List<EftTransactionResponse>>> Handle(GetAllEftTransactionQuery request,
        CancellationToken cancellationToken)
    {
        var list = await dbContext.Set<EftTransaction>()
            .Include(x => x.Account)
            // .Include(x => x.EftTransactionTransactions) //Bunları içermese de olur
            // .Include(x => x.EftTransactions)
            .ToListAsync(cancellationToken);
        
        var mappedList = mapper.Map<List<EftTransaction>, List<EftTransactionResponse>>(list);
         return new ApiResponse<List<EftTransactionResponse>>(mappedList);
    }

    //by id
    public async Task<ApiResponse<EftTransactionResponse>> Handle(GetEftTransactionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity =  await dbContext.Set<EftTransaction>()
           .Include(x => x.Account)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            return new ApiResponse<EftTransactionResponse>("Record not found");
        }
        
        var mapped = mapper.Map<EftTransaction, EftTransactionResponse>(entity);
        return new ApiResponse<EftTransactionResponse>(mapped);
    }

    //parametre değerlerine göre sorgular 
    public async Task<ApiResponse<List<EftTransactionResponse>>> Handle(GetEftTransactionByParameterQuery request,
        CancellationToken cancellationToken)
    {
        var list =  await dbContext.Set<EftTransaction>()
           .Include(x => x.Account)
            .Where(x =>
            x.Description.ToUpper().Contains(request.SenderIban.ToUpper()) ||
            (x.TransactionDate==request.TransactionDate)
        ).ToListAsync(cancellationToken);
        
        var mappedList = mapper.Map<List<EftTransaction>, List<EftTransactionResponse>>(list);
        return new ApiResponse<List<EftTransactionResponse>>(mappedList);
    }
}