using System.Security.Claims;
using AutoMapper;
using Grpc.Core;
using GsServer.Models;
using GsServer.Protobufs;

namespace GsServer.Services;

public class SaleRpcService : SaleService.SaleServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<SaleRpcService> _logger;
  private readonly IMapper _mapper;
  public SaleRpcService(
      ILogger<SaleRpcService> logger,
      DatabaseContext dbContext,
      IMapper mapper
    )
  {
    _logger = logger;
    _dbContext = dbContext;
    _mapper = mapper;
  }

  public override async Task<GetPaginatedSalesResponse> GetPaginatedAsync(GetPaginatedSalesRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing multiple records ({RecordType}) with cursor {Cursor}",
      RequestTracerId,
      UserId,
      typeof(Sale).Name,
      request.Cursor
    );

    IQueryable<GetSaleByIdResponse> Query = _dbContext.Sales.Select(
      Sale => _mapper.Map<GetSaleByIdResponse>(Sale)
    );

    // TODO
    // IQueryable<GetSaleByIdResponse> Query = _dbContext.Sales.Select(
    //   Sale => new GetSaleByIdResponse
    //   {
    //     TODO
    //   }
    // );

    List<GetSaleByIdResponse> Sales = [];

    /// If cursor is bigger than the size of the collection you will get the following error
    /// ArgumentOutOfRangeException "Index was out of range. Must be non-negative and less than the size of the collection"
    Sales = await Query
      .Where(x => x.SaleId > request.Cursor)
      .Take(20)
      .ToListAsync();

    GetPaginatedSalesResponse response = new();

    response.Sales.AddRange(Sales);
    if (Sales.Count < 20)
    {
      /// Avoiding `ArgumentOutOfRangeException`, basically, don't fetch if null
      response.NextCursor = null;
    }
    else
    {
      /// Id of the last element of the list, same value as `Users[Users.Count - 1].Id`
      response.NextCursor = Sales[^1].SaleId;
    }

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
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

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

    return _mapper.Map<GetSaleByIdResponse>(Sale);

    // TODO
    // return new GetSaleByIdResponse
    // {
    //   TODO
    // };
  }

  public override async Task<CreateSaleResponse> PostAsync(CreateSaleRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} creating new record ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(Sale).Name
    );

    Sale Sale = _mapper.Map<Sale>(request);
    Sale.CreatedBy = UserId;

    // TODO
    // var Sale = new Sale
    // {
    //   CustomerFk = request.CustomerFk,
    //   Comments = request.Comments,
    //   ItemsSold = request.ItemsSold.Select(
    //     ItemSold => new Models.SaleItem
    //     {
    //       ProductVariantFk = ItemSold.ProductVariantFk,
    //       QuantitySold = ItemSold.QuantitySold,
    //     }
    //   ).ToList(),
    //   CreatedBy = UserId,
    // };

    await _dbContext.AddAsync(Sale);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) created successfully, RecordId {RecordId}",
      RequestTracerId,
      typeof(Sale).Name,
      Sale.SaleId
    );

    return new CreateSaleResponse();
  }

  public override Task<UpdateSaleResponse> PutAsync(UpdateSaleRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );
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
    // if (request.Id <= 0)
    //   throw new RpcException(new Status(StatusCode.InvalidArgument, "You must supply a valid id"));

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

  public override async Task<DeleteSaleResponse> DeleteAsync(DeleteSaleRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );
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

    return new DeleteSaleResponse();
  }
}
