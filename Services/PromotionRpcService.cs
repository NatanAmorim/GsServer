using System.Security.Claims;
using AutoMapper;
using Grpc.Core;
using GsServer.Models;
using GsServer.Protobufs;

namespace GsServer.Services;

public class PromotionRpcService : PromotionService.PromotionServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<PromotionRpcService> _logger;
  private readonly IMapper _mapper;
  public PromotionRpcService(
      ILogger<PromotionRpcService> logger,
      DatabaseContext dbContext,
      IMapper mapper
    )
  {
    _logger = logger;
    _dbContext = dbContext;
    _mapper = mapper;
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

    IQueryable<GetPromotionByIdResponse> Query = _dbContext.Promotions.Select(
      Promotion => _mapper.Map<GetPromotionByIdResponse>(Promotion)
    );

    List<GetPromotionByIdResponse> Promotions = [];

    /// If cursor is bigger than the size of the collection you will get the following error
    /// ArgumentOutOfRangeException "Index was out of range. Must be non-negative and less than the size of the collection"
    Promotions = await Query
      .Where(x => x.PromotionId.CompareTo(Ulid.Parse(request.Cursor)) > 0)
      .Take(20)
      .ToListAsync();

    GetPaginatedPromotionsResponse response = new();

    response.Promotions.AddRange(Promotions);
    if (Promotions.Count < 20)
    {
      /// Avoiding `ArgumentOutOfRangeException`, basically, don't fetch if null
      response.NextCursor = null;
    }
    else
    {
      /// Id of the last element of the list, same value as `Users[Users.Count - 1].Id`
      response.NextCursor = Promotions[^1].PromotionId;
    }

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

    return _mapper.Map<GetPromotionByIdResponse>(Promotion);
  }

  public override async Task<CreatePromotionResponse> PostAsync(CreatePromotionRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} creating new record ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(Promotion).Name
    );

    Promotion Promotion = _mapper.Map<Promotion>(request);
    Promotion.CreatedBy = Ulid.Parse(UserId);

    await _dbContext.AddAsync(Promotion);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) created successfully, RecordId {RecordId}",
      RequestTracerId,
      typeof(Promotion).Name,
      Promotion.PromotionId
    );

    return new CreatePromotionResponse();
  }

  public override Task<UpdatePromotionResponse> PutAsync(UpdatePromotionRequest request, ServerCallContext context)
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

  public override async Task<DeletePromotionResponse> DeleteAsync(DeletePromotionRequest request, ServerCallContext context)
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

    return new DeletePromotionResponse();
  }
}
