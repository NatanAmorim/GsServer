using System.Security.Claims;
using AutoMapper;
using Grpc.Core;
using GsServer.Models;
using GsServer.Protobufs;

namespace GsServer.Services;

public class ReturnRpcService : ReturnService.ReturnServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<ReturnRpcService> _logger;
  private readonly IMapper _mapper;
  public ReturnRpcService(
      ILogger<ReturnRpcService> logger,
      DatabaseContext dbContext,
      IMapper mapper
    )
  {
    _logger = logger;
    _dbContext = dbContext;
    _mapper = mapper;
  }

  public override async Task<GetPaginatedReturnsResponse> GetPaginatedAsync(GetPaginatedReturnsRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing multiple records ({RecordType}) with cursor {Cursor}",
      RequestTracerId,
      UserId,
      typeof(Return).Name,
      request.Cursor
    );

    IQueryable<GetReturnByIdResponse> Query = _dbContext.Returns.Select(
      Return => _mapper.Map<GetReturnByIdResponse>(Return)
    );

    // TODO
    // IQueryable<GetReturnByIdResponse> Query = _dbContext.Returns.Select(
    //   Return => new GetReturnByIdResponse
    //   {
    //     TotalAmountRefunded = Return.TotalAmountRefunded,
    //     ItemsReturned =
    //     {
    //       Return.ItemsReturned.Select(
    //         ItemReturned => new ReturnItem
    //         {
    //           ProductVariantFk = ItemReturned.ProductVariantFk,
    //           QuantityReturned = ItemReturned.QuantityReturned,
    //         }
    //       ).ToList(),
    //     }
    //   }
    // );

    List<GetReturnByIdResponse> Returns = [];

    /// If cursor is bigger than the size of the collection you will get the following error
    /// ArgumentOutOfRangeException "Index was out of range. Must be non-negative and less than the size of the collection"
    Returns = await Query
      .Where(x => x.ReturnId > request.Cursor)
      .Take(20)
      .ToListAsync();

    GetPaginatedReturnsResponse response = new();

    response.Returns.AddRange(Returns);
    if (Returns.Count < 20)
    {
      /// Avoiding `ArgumentOutOfRangeException`, basically, don't fetch if null
      response.NextCursor = null;
    }
    else
    {
      /// Id of the last element of the list, same value as `Users[Users.Count - 1].Id`
      response.NextCursor = Returns[^1].ReturnId;
    }

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
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

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

    return _mapper.Map<GetReturnByIdResponse>(Return);

    // TODO
    // return new GetReturnByIdResponse
    // {
    //   TotalAmountRefunded = Return.TotalAmountRefunded,
    //   ItemsReturned =
    //   {
    //     Return.ItemsReturned.Select(
    //       ItemReturned => new ReturnItem
    //       {
    //         ProductVariantFk = ItemReturned.ProductVariantFk,
    //         QuantityReturned = ItemReturned.QuantityReturned,
    //       }
    //     ).ToList(),
    //   }
    // };
  }

  public override async Task<CreateReturnResponse> PostAsync(CreateReturnRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} creating new record ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(Return).Name
    );

    Return Return = _mapper.Map<Return>(request);
    Return.CreatedBy = UserId;

    // TODO
    // var Return = new Return
    // {
    //   TotalAmountRefunded = request.TotalAmountRefunded,
    //   ItemsReturned = request.ItemsReturned.Select(
    //       ItemReturned => new ReturnItem
    //       {
    //         ProductVariantFk = ItemReturned.ProductVariantFk,
    //         QuantityReturned = ItemReturned.QuantityReturned,
    //       }
    //     ).ToList(),
    //   CreatedBy = UserId,
    // };

    await _dbContext.AddAsync(Return);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) created successfully, RecordId {RecordId}",
      RequestTracerId,
      typeof(Return).Name,
      Return.ReturnId
    );

    return new CreateReturnResponse();
  }

  public override Task<UpdateReturnResponse> PutAsync(UpdateReturnRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );
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
    // if (request.Id <= 0)
    //   throw new RpcException(new Status(StatusCode.InvalidArgument, "You must supply a valid id"));

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

  public override async Task<DeleteReturnResponse> DeleteAsync(DeleteReturnRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );
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

    return new DeleteReturnResponse();
  }
}
