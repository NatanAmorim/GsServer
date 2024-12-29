using System.Security.Claims;
using Grpc.Core;
using GsServer.Models;
using GsServer.Protobufs;
using Microsoft.AspNetCore.Authorization;

namespace GsServer.Services;

[Authorize]
public class SubscriptionBillingRpcService : SubscriptionBillingService.SubscriptionBillingServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<SubscriptionBillingRpcService> _logger;
  public SubscriptionBillingRpcService(
      ILogger<SubscriptionBillingRpcService> logger,
      DatabaseContext dbContext
    )
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override async Task<GetPaginatedSubscriptionBillingsResponse> GetPaginatedAsync(GetPaginatedSubscriptionBillingsRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing multiple records ({RecordType}) with cursor {Cursor}",
      RequestTracerId,
      UserId,
      typeof(SubscriptionBilling).Name,
      request.Cursor
    );

    IQueryable<GetSubscriptionBillingByIdResponse> Query;

    if (request.Cursor is null || request.Cursor == string.Empty)
    {
      Query = _dbContext.SubscriptionBillings
        .Select(SubscriptionBilling => SubscriptionBilling.ToGetById());
    }
    else
    {
      Query = _dbContext.SubscriptionBillings
        .Where(x => x.SubscriptionBillingId.CompareTo(Ulid.Parse(request.Cursor)) > 0)
        .Select(SubscriptionBilling => SubscriptionBilling.ToGetById());
    }

    List<GetSubscriptionBillingByIdResponse> SubscriptionBillings = await Query
      .Take(20)
      .AsNoTracking()
      .ToListAsync();

    GetPaginatedSubscriptionBillingsResponse response = new();

    response.SubscriptionBillings.AddRange(SubscriptionBillings);
    response.NextCursor = SubscriptionBillings.LastOrDefault()?.SubscriptionBillingId;

    _logger.LogInformation(
      "({TraceIdentifier}) multiple records ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(SubscriptionBilling).Name
    );

    return response;
  }

  public override async Task<GetSubscriptionBillingByIdResponse> GetByIdAsync(GetSubscriptionBillingByIdRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(SubscriptionBilling).Name,
      request.SubscriptionBillingId
    );

    SubscriptionBilling? SubscriptionBilling = await _dbContext.SubscriptionBillings.FindAsync(request.SubscriptionBillingId);

    if (SubscriptionBilling is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) record ({RecordType}) not found",
        RequestTracerId,
        typeof(SubscriptionBilling).Name
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Nenhum produto com ID {request.SubscriptionBillingId}"
      ));
    }

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(SubscriptionBilling).Name
    );

    return SubscriptionBilling.ToGetById();
  }

  public override async Task<VoidValue> PostAsync(CreateSubscriptionBillingRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} creating new record ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(SubscriptionBilling).Name
    );

    SubscriptionBilling SubscriptionBilling = SubscriptionBilling.FromProtoRequest(request, Ulid.Parse(UserId));

    await _dbContext.AddAsync(SubscriptionBilling);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) created successfully, RecordId {RecordId}",
      RequestTracerId,
      typeof(SubscriptionBilling).Name,
      SubscriptionBilling.SubscriptionBillingId
    );

    return new VoidValue();
  }

  public override Task<VoidValue> PutAsync(UpdateSubscriptionBillingRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} updating record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(SubscriptionBilling).Name,
      request.SubscriptionBillingId
    );

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) updated successfully",
      RequestTracerId,
      typeof(SubscriptionBilling).Name
    );

    throw new NotImplementedException();

    // TODO

    // SubscriptionBillingModel? SubscriptionBilling = await _dbContext.SubscriptionBillings.FirstOrDefaultAsync(x => x.Id == request.Id);
    // if (SubscriptionBilling is null)
    // {
    //   throw new RpcException(new Status(
    //     StatusCode.NotFound, $"registro não encontrado"
    //   ));
    // }

    // SubscriptionBilling.Name = request.Name;
    // // TODO Add Another fields

    // await _dbContext.SaveChangesAsync();
    // // TODO Log => Record (record type) ID Y was updated. Old value of (field name): (old value). New value: (new value). (This logs specific changes made to a field within a record)
    // return new UpdateSubscriptionBillingResponse();
  }

  public override async Task<VoidValue> DeleteAsync(DeleteSubscriptionBillingRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
        "({TraceIdentifier}) User {UserID} deleting record ({RecordType}) with ID ({RecordId})",
        RequestTracerId,
        UserId,
        typeof(SubscriptionBilling).Name,
        request.SubscriptionBillingId
      );

    SubscriptionBilling? SubscriptionBilling = await _dbContext.SubscriptionBillings.FindAsync(request.SubscriptionBillingId);

    if (SubscriptionBilling is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) Error deleting record ({RecordType}) with ID {Id}, record not found",
        RequestTracerId,
        typeof(SubscriptionBilling).Name,
        request.SubscriptionBillingId
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Erro ao remover registro, nenhum registro com ID {request.SubscriptionBillingId}"
      ));
    }

    /// TODO check if record is being used before deleting it use something like PK or FK

    _dbContext.SubscriptionBillings.Remove(SubscriptionBilling);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
          "({TraceIdentifier}) record ({RecordType}) deleted successfully",
          RequestTracerId,
          typeof(SubscriptionBilling).Name
        );

    return new VoidValue();
  }
}
