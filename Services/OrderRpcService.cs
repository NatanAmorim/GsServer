using Grpc.Core;
using GsServer.Protobufs;

namespace GsServer.Services;

public class OrderRpcService : OrderService.OrderServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<OrderRpcService> _logger;
  public OrderRpcService(ILogger<OrderRpcService> logger, DatabaseContext dbContext)
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override Task<GetPaginatedOrdersResponse> GetPaginated(GetPaginatedOrdersRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<GetOrderByIdResponse> GetById(GetOrderByIdRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<CreateOrderResponse> Post(CreateOrderRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<UpdateOrderResponse> Put(UpdateOrderRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<DeleteOrderResponse> Delete(DeleteOrderRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }
}
