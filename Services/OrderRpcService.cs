using Grpc.Core;
using gs_server.Protobufs;

namespace gs_server.Services;

public class OrderRpcService : OrderService.OrderServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<OrderRpcService> _logger;
  public OrderRpcService(ILogger<OrderRpcService> logger, DatabaseContext dbContext)
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override Task<GetAllOrdersResponse> GetAll(GetAllOrdersRequest request, ServerCallContext context)
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
