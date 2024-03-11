using Grpc.Core;
using gs_server.Protobufs;

namespace gs_server.Services;

public class SaleRpcService : SaleService.SaleServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<SaleRpcService> _logger;
  public SaleRpcService(ILogger<SaleRpcService> logger, DatabaseContext dbContext)
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override Task<GetPaginatedSalesResponse> GetPaginated(GetPaginatedSalesRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<GetSaleByIdResponse> GetById(GetSaleByIdRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<CreateSaleResponse> Post(CreateSaleRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<UpdateSaleResponse> Put(UpdateSaleRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<DeleteSaleResponse> Delete(DeleteSaleRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }
}
