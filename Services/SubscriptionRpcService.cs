using System.Security.Claims;
using Grpc.Core;
using GsServer.Models;
using GsServer.Protobufs;
using Microsoft.AspNetCore.Authorization;

namespace GsServer.Services;

[Authorize]
public class SubscriptionRpcService : SubscriptionService.SubscriptionServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<SubscriptionRpcService> _logger;
  public SubscriptionRpcService(
      ILogger<SubscriptionRpcService> logger,
      DatabaseContext dbContext
    )
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override async Task<GetPaginatedSubscriptionsResponse> GetPaginatedAsync(GetPaginatedSubscriptionsRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing multiple records ({RecordType}) with cursor {Cursor}",
      RequestTracerId,
      UserId,
      typeof(Subscription).Name,
      request.Cursor
    );

    IQueryable<GetSubscriptionByIdResponse> Query;

    if (request.Cursor is null)
    {
      Query = _dbContext.Subscriptions
        .Select(Subscription => Subscription.ToGetById());
    }
    else
    {
      Query = _dbContext.Subscriptions
        .Where(x => x.SubscriptionId.CompareTo(Ulid.Parse(request.Cursor)) > 0)
        .Select(Subscription => Subscription.ToGetById());
    }

    List<GetSubscriptionByIdResponse> Subscriptions = await Query
      .Take(20)
      .ToListAsync();

    GetPaginatedSubscriptionsResponse response = new();

    response.Subscriptions.AddRange(Subscriptions);
    response.NextCursor = Subscriptions.LastOrDefault()?.SubscriptionId;

    _logger.LogInformation(
      "({TraceIdentifier}) multiple records ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(Subscription).Name
    );

    return response;
  }

  public override async Task<GetSubscriptionByIdResponse> GetByIdAsync(GetSubscriptionByIdRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Subscription).Name,
      request.SubscriptionId
    );

    Subscription? Subscription = await _dbContext.Subscriptions.FindAsync(request.SubscriptionId);

    if (Subscription is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) record ({RecordType}) not found",
        RequestTracerId,
        typeof(Subscription).Name
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Nenhum produto com ID {request.SubscriptionId}"
      ));
    }

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(Subscription).Name
    );

    return Subscription.ToGetById();
  }

  public override async Task<VoidValue> PostAsync(CreateSubscriptionRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} creating new record ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(Subscription).Name
    );

    Subscription Subscription = Subscription.FromProtoRequest(request, Ulid.Parse(UserId));

    await _dbContext.AddAsync(Subscription);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) created successfully, RecordId {RecordId}",
      RequestTracerId,
      typeof(Subscription).Name,
      Subscription.SubscriptionId
    );

    return new VoidValue();
  }

  public override Task<VoidValue> PutAsync(UpdateSubscriptionRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} updating record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Subscription).Name,
      request.SubscriptionId
    );

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) updated successfully",
      RequestTracerId,
      typeof(Subscription).Name
    );

    throw new NotImplementedException();

    // TODO

    // SubscriptionModel? Subscription = await _dbContext.Subscriptions.FirstOrDefaultAsync(x => x.Id == request.Id);
    // if (Subscription is null)
    // {
    //   throw new RpcException(new Status(
    //     StatusCode.NotFound, $"registro não encontrado"
    //   ));
    // }

    // Subscription.Name = request.Name;
    // // TODO Add Another fields

    // await _dbContext.SaveChangesAsync();
    // // TODO Log => Record (record type) ID Y was updated. Old value of (field name): (old value). New value: (new value). (This logs specific changes made to a field within a record)
    // return new UpdateSubscriptionResponse();
  }

  public override async Task<VoidValue> DeleteAsync(DeleteSubscriptionRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
        "({TraceIdentifier}) User {UserID} deleting record ({RecordType}) with ID ({RecordId})",
        RequestTracerId,
        UserId,
        typeof(Subscription).Name,
        request.SubscriptionId
      );

    Subscription? Subscription = await _dbContext.Subscriptions.FindAsync(request.SubscriptionId);

    if (Subscription is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) Error deleting record ({RecordType}) with ID {Id}, record not found",
        RequestTracerId,
        typeof(Subscription).Name,
        request.SubscriptionId
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Erro ao remover registro, nenhum registro com ID {request.SubscriptionId}"
      ));
    }

    /// TODO check if record is being used before deleting it use something like PK or FK

    _dbContext.Subscriptions.Remove(Subscription);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
          "({TraceIdentifier}) record ({RecordType}) deleted successfully",
          RequestTracerId,
          typeof(Subscription).Name
        );

    return new VoidValue();
  }
}
