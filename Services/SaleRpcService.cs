using System.Security.Claims;
using Grpc.Core;
using GsServer.Models;
using GsServer.Protobufs;
using Microsoft.AspNetCore.Authorization;

namespace GsServer.Services;

[Authorize]
public class SaleRpcService : SaleService.SaleServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<SaleRpcService> _logger;
  public SaleRpcService(
      ILogger<SaleRpcService> logger,
      DatabaseContext dbContext
    )
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override async Task<GetPaginatedSalesResponse> GetPaginatedAsync(GetPaginatedSalesRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing multiple records ({RecordType}) with cursor {Cursor}",
      RequestTracerId,
      UserId,
      typeof(Sale).Name,
      request.Cursor
    );

    IQueryable<GetSaleByIdResponse> Query;

    if (request.Cursor is null || request.Cursor == string.Empty)
    {
      Query = _dbContext.Sales
        .Select(Sale => Sale.ToGetById());
    }
    else
    {
      Query = _dbContext.Sales
        .Where(x => x.SaleId.CompareTo(Ulid.Parse(request.Cursor)) > 0)
        .Select(Sale => Sale.ToGetById());
    }

    List<GetSaleByIdResponse> Sales = await Query
      .Take(20)
      .AsNoTracking()
      .ToListAsync();

    GetPaginatedSalesResponse response = new();

    response.Sales.AddRange(Sales);
    response.NextCursor = Sales.LastOrDefault()?.SaleId;

    _logger.LogInformation(
      "({TraceIdentifier}) multiple records ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(Sale).Name
    );

    return response;
  }

  public override async Task<GetSaleByIdResponse> GetByIdAsync(GetSaleByIdRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Sale).Name,
      request.SaleId
    );

    Sale? Sale = await _dbContext.Sales.FindAsync(request.SaleId);

    if (Sale is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) record ({RecordType}) not found",
        RequestTracerId,
        typeof(Sale).Name
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Nenhum produto com ID {request.SaleId}"
      ));
    }

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(Sale).Name
    );

    return Sale.ToGetById();
  }

  public override async Task<VoidValue> PostAsync(CreateSaleRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} creating new record ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(Sale).Name
    );

    Sale Sale = Sale.FromProtoRequest(request, Ulid.Parse(UserId));

    await _dbContext.AddAsync(Sale);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) created successfully, RecordId {RecordId}",
      RequestTracerId,
      typeof(Sale).Name,
      Sale.SaleId
    );

    return new VoidValue();
  }

  public override Task<VoidValue> PutAsync(UpdateSaleRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} updating record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Sale).Name,
      request.SaleId
    );

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) updated successfully",
      RequestTracerId,
      typeof(Sale).Name
    );

    throw new NotImplementedException();

    // TODO

    // SaleModel? Sale = await _dbContext.Sales.FirstOrDefaultAsync(x => x.Id == request.Id);
    // if (Sale is null)
    // {
    //   throw new RpcException(new Status(
    //     StatusCode.NotFound, $"registro não encontrado"
    //   ));
    // }

    // Sale.Name = request.Name;
    // // TODO Add Another fields

    // await _dbContext.SaveChangesAsync();
    // // TODO Log => Record (record type) ID Y was updated. Old value of (field name): (old value). New value: (new value). (This logs specific changes made to a field within a record)
    // return new UpdateSaleResponse();
  }

  public override async Task<VoidValue> DeleteAsync(DeleteSaleRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
        "({TraceIdentifier}) User {UserID} deleting record ({RecordType}) with ID ({RecordId})",
        RequestTracerId,
        UserId,
        typeof(Sale).Name,
        request.SaleId
      );

    Sale? Sale = await _dbContext.Sales.FindAsync(request.SaleId);

    if (Sale is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) Error deleting record ({RecordType}) with ID {Id}, record not found",
        RequestTracerId,
        typeof(Sale).Name,
        request.SaleId
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Erro ao remover registro, nenhum registro com ID {request.SaleId}"
      ));
    }

    /// TODO check if record is being used before deleting it use something like PK or FK

    _dbContext.Sales.Remove(Sale);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
          "({TraceIdentifier}) record ({RecordType}) deleted successfully",
          RequestTracerId,
          typeof(Sale).Name
        );

    return new VoidValue();
  }
}
