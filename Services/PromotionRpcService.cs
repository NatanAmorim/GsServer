using System.Security.Claims;
using Grpc.Core;
using GsServer.Models;
using GsServer.Protobufs;
using Microsoft.AspNetCore.Authorization;

namespace GsServer.Services;

[Authorize]
public class PromotionRpcService : PromotionService.PromotionServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<PromotionRpcService> _logger;
  public PromotionRpcService(
      ILogger<PromotionRpcService> logger,
      DatabaseContext dbContext
    )
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override async Task<GetPaginatedPromotionsResponse> GetPaginatedAsync(GetPaginatedPromotionsRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing multiple records ({RecordType}) with cursor {Cursor}",
      RequestTracerId,
      UserId,
      typeof(Promotion).Name,
      request.Cursor
    );

    IQueryable<GetPromotionByIdResponse> Query;

    if (request.Cursor is null)
    {
      Query = _dbContext.Promotions
        .Select(Promotion => Promotion.ToGetById());
    }
    else
    {
      Query = _dbContext.Promotions
        .Where(x => x.PromotionId.CompareTo(Ulid.Parse(request.Cursor)) > 0)
        .Select(Promotion => Promotion.ToGetById());
    }

    List<GetPromotionByIdResponse> Promotions = await Query
      .Take(20)
      .ToListAsync();

    GetPaginatedPromotionsResponse response = new();

    response.Promotions.AddRange(Promotions);
    response.NextCursor = Promotions.LastOrDefault()?.PromotionId;

    _logger.LogInformation(
      "({TraceIdentifier}) multiple records ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(Promotion).Name
    );

    return response;
  }

  public override async Task<GetPromotionByIdResponse> GetByIdAsync(GetPromotionByIdRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Promotion).Name,
      request.PromotionId
    );

    Promotion? Promotion = await _dbContext.Promotions.FindAsync(request.PromotionId);

    if (Promotion is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) record ({RecordType}) not found",
        RequestTracerId,
        typeof(Promotion).Name
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Nenhum produto com ID {request.PromotionId}"
      ));
    }

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(Promotion).Name
    );

    return Promotion.ToGetById();
  }

  public override async Task<Protobufs.Void> PostAsync(CreatePromotionRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} creating new record ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(Promotion).Name
    );

    Promotion Promotion = Promotion.FromProtoRequest(request, Ulid.Parse(UserId));

    await _dbContext.AddAsync(Promotion);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) created successfully, RecordId {RecordId}",
      RequestTracerId,
      typeof(Promotion).Name,
      Promotion.PromotionId
    );

    return new Protobufs.Void();
  }

  public override Task<Protobufs.Void> PutAsync(UpdatePromotionRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} updating record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Promotion).Name,
      request.PromotionId
    );

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) updated successfully",
      RequestTracerId,
      typeof(Promotion).Name
    );

    throw new NotImplementedException();

    // TODO
    // PromotionModel? Promotion = await _dbContext.Promotions.FirstOrDefaultAsync(x => x.Id == request.Id);
    // if (Promotion is null)
    // {
    //   throw new RpcException(new Status(
    //     StatusCode.NotFound, $"registro não encontrado"
    //   ));
    // }

    // Promotion.Name = request.Name;
    // // TODO Add Another fields

    // await _dbContext.SaveChangesAsync();
    // // TODO Log => Record (record type) ID Y was updated. Old value of (field name): (old value). New value: (new value). (This logs specific changes made to a field within a record)
    // return new UpdatePromotionResponse();
  }

  public override async Task<Protobufs.Void> DeleteAsync(DeletePromotionRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
        "({TraceIdentifier}) User {UserID} deleting record ({RecordType}) with ID ({RecordId})",
        RequestTracerId,
        UserId,
        typeof(Promotion).Name,
        request.PromotionId
      );

    Promotion? Promotion = await _dbContext.Promotions.FindAsync(request.PromotionId);

    if (Promotion is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) Error deleting record ({RecordType}) with ID {Id}, record not found",
        RequestTracerId,
        typeof(Promotion).Name,
        request.PromotionId
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Erro ao remover registro, nenhum registro com ID {request.PromotionId}"
      ));
    }

    /// TODO check if record is being used before deleting it use something like PK or FK

    _dbContext.Promotions.Remove(Promotion);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
          "({TraceIdentifier}) record ({RecordType}) deleted successfully",
          RequestTracerId,
          typeof(Promotion).Name
        );

    return new Protobufs.Void();
  }
}
