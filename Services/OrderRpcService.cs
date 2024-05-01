using System.Security.Claims;
using Grpc.Core;
using GsServer.Models;
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

  public override async Task<GetPaginatedOrdersResponse> GetPaginatedAsync(GetPaginatedOrdersRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing multiple records ({RecordType}) with cursor {Cursor}",
      RequestTracerId,
      UserId,
      typeof(Order).Name,
      request.Cursor
    );
    IQueryable<GetOrderByIdResponse> Query = _dbContext.Orders.Select(
      Order => new GetOrderByIdResponse
      {
        // TODO
      }
    );

    List<GetOrderByIdResponse> Orders = [];

    /// If cursor is bigger than the size of the collection you will get the following error
    /// ArgumentOutOfRangeException "Index was out of range. Must be non-negative and less than the size of the collection"
    Orders = await Query
      .Where(x => x.OrderId.CompareTo(Ulid.Parse(request.Cursor)) > 0)
      .Take(20)
      .ToListAsync();

    GetPaginatedOrdersResponse response = new();

    response.Orders.AddRange(Orders);
    if (Orders.Count < 20)
    {
      /// Avoiding `ArgumentOutOfRangeException`, basically, don't fetch if null
      response.NextCursor = null;
    }
    else
    {
      /// Id of the last element of the list, same value as `Users[Users.Count - 1].Id`
      response.NextCursor = Orders[^1].OrderId;
    }

    _logger.LogInformation(
      "({TraceIdentifier}) multiple records ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(Order).Name
    );
    return response;
  }

  public override async Task<GetOrderByIdResponse> GetByIdAsync(GetOrderByIdRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Order).Name,
      request.OrderId
    );

    Order? Order = await _dbContext.Orders.FindAsync(request.OrderId);

    if (Order is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) record ({RecordType}) not found",
        RequestTracerId,
        typeof(Order).Name
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Nenhum produto com ID {request.OrderId}"
      ));
    }

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(Order).Name
    );

    return new GetOrderByIdResponse
    {
      // TODO
    };
  }

  public override async Task<CreateOrderResponse> PostAsync(CreateOrderRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} creating new record ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(Order).Name
    );

    // TODO
    // var Order = new OrderModel
    // {
    // };

    // await _dbContext.AddAsync(Order);
    // await _dbContext.SaveChangesAsync();

    // _logger.LogInformation(
    //   "({TraceIdentifier}) record ({RecordType}) created successfully, RecordId {RecordId}",
    //   RequestTracerId,
    //   typeof(OrderModel).Name,
    //   Order.OrderId
    // );

    return new CreateOrderResponse();
  }

  public override Task<UpdateOrderResponse> PutAsync(UpdateOrderRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} updating record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Order).Name,
      request.OrderId
    );

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) updated successfully",
      RequestTracerId,
      typeof(Order).Name
    );

    throw new NotImplementedException();

    // TODO
    // OrderModel? Order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == request.Id);
    // if (Order is null)
    // {
    //   throw new RpcException(new Status(
    //     StatusCode.NotFound, $"registro não encontrado"
    //   ));
    // }

    // Order.Name = request.Name;
    // // TODO Add Another fields

    // await _dbContext.SaveChangesAsync();
    // // TODO Log => Record (record type) ID Y was updated. Old value of (field name): (old value). New value: (new value). (This logs specific changes made to a field within a record)
    // return new UpdateOrderResponse();
  }

  public override async Task<DeleteOrderResponse> DeleteAsync(DeleteOrderRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
        "({TraceIdentifier}) User {UserID} deleting record ({RecordType}) with ID ({RecordId})",
        RequestTracerId,
        UserId,
        typeof(Order).Name,
        request.OrderId
      );

    Order? Order = await _dbContext.Orders.FindAsync(request.OrderId);

    if (Order is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) Error deleting record ({RecordType}) with ID {Id}, record not found",
        RequestTracerId,
        typeof(Order).Name,
        request.OrderId
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Erro ao remover registro, nenhum registro com ID {request.OrderId}"
      ));
    }

    /// TODO check if record is being used before deleting it use something like PK or FK

    _dbContext.Orders.Remove(Order);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
          "({TraceIdentifier}) record ({RecordType}) deleted successfully",
          RequestTracerId,
          typeof(Order).Name
        );

    return new DeleteOrderResponse();
  }
}
