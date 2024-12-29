using System.Security.Claims;
using Grpc.Core;
using GsServer.Models;
using GsServer.Protobufs;
using Microsoft.AspNetCore.Authorization;

namespace GsServer.Services;

[Authorize]
public class ReturnRpcService : ReturnService.ReturnServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<ReturnRpcService> _logger;
  public ReturnRpcService(
      ILogger<ReturnRpcService> logger,
      DatabaseContext dbContext
    )
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override async Task<GetPaginatedReturnsResponse> GetPaginatedAsync(GetPaginatedReturnsRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing multiple records ({RecordType}) with cursor {Cursor}",
      RequestTracerId,
      UserId,
      typeof(Return).Name,
      request.Cursor
    );

    IQueryable<GetReturnByIdResponse> Query;

    if (request.Cursor is null || request.Cursor == string.Empty)
    {
      Query = _dbContext.Returns
        .Select(
          Return => Return.ToGetById()
        );
    }
    else
    {
      Query = _dbContext.Returns
        .Where(x => x.ReturnId.CompareTo(Ulid.Parse(request.Cursor)) > 0)
        .Select(
          Return => Return.ToGetById()
        );
    }

    List<GetReturnByIdResponse> Returns = await Query
      .Take(20)
      .AsNoTracking()
      .ToListAsync();

    GetPaginatedReturnsResponse response = new();

    response.Returns.AddRange(Returns);
    response.NextCursor = Returns.LastOrDefault()?.ReturnId;

    _logger.LogInformation(
      "({TraceIdentifier}) multiple records ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(Return).Name
    );

    return response;
  }

  public override async Task<GetReturnByIdResponse> GetByIdAsync(GetReturnByIdRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Return).Name,
      request.ReturnId
    );

    Return? Return = await _dbContext.Returns.FindAsync(request.ReturnId);

    if (Return is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) record ({RecordType}) not found",
        RequestTracerId,
        typeof(Return).Name
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Nenhum produto com ID {request.ReturnId}"
      ));
    }

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(Return).Name
    );

    return Return.ToGetById();
  }

  public override async Task<VoidValue> PostAsync(CreateReturnRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} creating new record ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(Return).Name
    );

    Return Return = Return.FromProtoRequest(request, Ulid.Parse(UserId));

    await _dbContext.AddAsync(Return);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) created successfully, RecordId {RecordId}",
      RequestTracerId,
      typeof(Return).Name,
      Return.ReturnId
    );

    return new VoidValue();
  }

  public override Task<VoidValue> PutAsync(UpdateReturnRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} updating record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Return).Name,
      request.ReturnId
    );

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) updated successfully",
      RequestTracerId,
      typeof(Return).Name
    );

    throw new NotImplementedException();

    // TODO
    // ReturnModel? Return = await _dbContext.Returns.FirstOrDefaultAsync(x => x.Id == request.Id);
    // if (Return is null)
    // {
    //   throw new RpcException(new Status(
    //     StatusCode.NotFound, $"registro não encontrado"
    //   ));
    // }

    // Return.Name = request.Name;
    // // TODO Add Another fields

    // await _dbContext.SaveChangesAsync();
    // // TODO Log => Record (record type) ID Y was updated. Old value of (field name): (old value). New value: (new value). (This logs specific changes made to a field within a record)
    // return new UpdateReturnResponse();
  }

  public override async Task<VoidValue> DeleteAsync(DeleteReturnRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
        "({TraceIdentifier}) User {UserID} deleting record ({RecordType}) with ID ({RecordId})",
        RequestTracerId,
        UserId,
        typeof(Return).Name,
        request.ReturnId
      );

    Return? Return = await _dbContext.Returns.FindAsync(request.ReturnId);

    if (Return is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) Error deleting record ({RecordType}) with ID {Id}, record not found",
        RequestTracerId,
        typeof(Return).Name,
        request.ReturnId
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Erro ao remover registro, nenhum registro com ID {request.ReturnId}"
      ));
    }

    /// TODO check if record is being used before deleting it use something like PK or FK

    _dbContext.Returns.Remove(Return);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
          "({TraceIdentifier}) record ({RecordType}) deleted successfully",
          RequestTracerId,
          typeof(Return).Name
        );

    return new VoidValue();
  }
}
