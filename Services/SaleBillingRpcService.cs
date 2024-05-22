using System.Security.Claims;
using Grpc.Core;
using GsServer.Models;
using GsServer.Protobufs;
using Microsoft.AspNetCore.Authorization;

namespace GsServer.Services;

[Authorize]
public class SaleBillingRpcService : SaleBillingService.SaleBillingServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<SaleBillingRpcService> _logger;
  public SaleBillingRpcService(
      ILogger<SaleBillingRpcService> logger,
      DatabaseContext dbContext
    )
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override async Task<GetPaginatedSaleBillingsResponse> GetPaginatedAsync(GetPaginatedSaleBillingsRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing multiple records ({RecordType}) with cursor {Cursor}",
      RequestTracerId,
      UserId,
      typeof(SaleBilling).Name,
      request.Cursor
    );

    IQueryable<GetSaleBillingByIdResponse> Query = _dbContext.SaleBillings.Select(
      SaleBilling => SaleBilling.ToGetById()
    );

    List<GetSaleBillingByIdResponse> SaleBillings = [];

    if (request.Cursor is null)
    {
      SaleBillings = await Query
       .Take(20)
       .ToListAsync();
    }
    else
    {
      /// If cursor is bigger than the size of the collection you will get the following error
      /// ArgumentOutOfRangeException "Index was out of range. Must be non-negative and less than the size of the collection"
      SaleBillings = await Query
       .Where(x => x.SaleBillingId.CompareTo(Ulid.Parse(request.Cursor)) > 0)
       .Take(20)
       .ToListAsync();
    }

    GetPaginatedSaleBillingsResponse response = new();

    response.SaleBillings.AddRange(SaleBillings);
    if (SaleBillings.Count < 20)
    {
      /// Avoiding `ArgumentOutOfRangeException`, basically, don't fetch if null
      response.NextCursor = null;
    }
    else
    {
      /// Id of the last element of the list, same value as `Users[Users.Count - 1].Id`
      response.NextCursor = SaleBillings[^1].SaleBillingId;
    }

    _logger.LogInformation(
      "({TraceIdentifier}) multiple records ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(SaleBilling).Name
    );
    return response;
  }

  public override async Task<GetSaleBillingByIdResponse> GetByIdAsync(GetSaleBillingByIdRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(SaleBilling).Name,
      request.SaleBillingId
    );

    SaleBilling? SaleBilling = await _dbContext.SaleBillings.FindAsync(request.SaleBillingId);

    if (SaleBilling is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) record ({RecordType}) not found",
        RequestTracerId,
        typeof(SaleBilling).Name
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Nenhum produto com ID {request.SaleBillingId}"
      ));
    }

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(SaleBilling).Name
    );

    return SaleBilling.ToGetById();
  }

  public override async Task<CreateSaleBillingResponse> PostAsync(CreateSaleBillingRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} creating new record ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(SaleBilling).Name
    );

    SaleBilling SaleBilling = SaleBilling.FromProtoRequest(request, Ulid.Parse(UserId));

    await _dbContext.AddAsync(SaleBilling);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) created successfully, RecordId {RecordId}",
      RequestTracerId,
      typeof(SaleBilling).Name,
      SaleBilling.SaleBillingId
    );

    return new CreateSaleBillingResponse();
  }

  public override Task<UpdateSaleBillingResponse> PutAsync(UpdateSaleBillingRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} updating record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(SaleBilling).Name,
      request.SaleBillingId
    );

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) updated successfully",
      RequestTracerId,
      typeof(SaleBilling).Name
    );

    throw new NotImplementedException();

    // TODO
    // SaleBillingModel? SaleBilling = await _dbContext.SaleBillings.FirstOrDefaultAsync(x => x.Id == request.Id);
    // if (SaleBilling is null)
    // {
    //   throw new RpcException(new Status(
    //     StatusCode.NotFound, $"registro não encontrado"
    //   ));
    // }

    // SaleBilling.Name = request.Name;
    // // TODO Add Another fields

    // await _dbContext.SaveChangesAsync();
    // // TODO Log => Record (record type) ID Y was updated. Old value of (field name): (old value). New value: (new value). (This logs specific changes made to a field within a record)
    // return new UpdateSaleBillingResponse();
  }

  public override async Task<DeleteSaleBillingResponse> DeleteAsync(DeleteSaleBillingRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
        "({TraceIdentifier}) User {UserID} deleting record ({RecordType}) with ID ({RecordId})",
        RequestTracerId,
        UserId,
        typeof(SaleBilling).Name,
        request.SaleBillingId
      );

    SaleBilling? SaleBilling = await _dbContext.SaleBillings.FindAsync(request.SaleBillingId);

    if (SaleBilling is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) Error deleting record ({RecordType}) with ID {Id}, record not found",
        RequestTracerId,
        typeof(SaleBilling).Name,
        request.SaleBillingId
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Erro ao remover registro, nenhum registro com ID {request.SaleBillingId}"
      ));
    }

    /// TODO check if record is being used before deleting it use something like PK or FK

    _dbContext.SaleBillings.Remove(SaleBilling);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
          "({TraceIdentifier}) record ({RecordType}) deleted successfully",
          RequestTracerId,
          typeof(SaleBilling).Name
        );

    return new DeleteSaleBillingResponse();
  }
}
