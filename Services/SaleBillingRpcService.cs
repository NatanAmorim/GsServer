using System.Security.Claims;
using AutoMapper;
using Grpc.Core;
using GsServer.Models;
using GsServer.Protobufs;

namespace GsServer.Services;

public class SaleBillingRpcService : SaleBillingService.SaleBillingServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<SaleBillingRpcService> _logger;
  private readonly IMapper _mapper;
  public SaleBillingRpcService(
      ILogger<SaleBillingRpcService> logger,
      DatabaseContext dbContext,
      IMapper mapper
    )
  {
    _logger = logger;
    _dbContext = dbContext;
    _mapper = mapper;
  }

  public override async Task<GetPaginatedSaleBillingsResponse> GetPaginatedAsync(GetPaginatedSaleBillingsRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing multiple records ({RecordType}) with cursor {Cursor}",
      RequestTracerId,
      UserId,
      typeof(SaleBilling).Name,
      request.Cursor
    );

    IQueryable<GetSaleBillingByIdResponse> Query = _dbContext.SaleBillings.Select(
      SaleBilling => _mapper.Map<GetSaleBillingByIdResponse>(SaleBilling)
    );

    // TODO
    // IQueryable<GetSaleBillingByIdResponse> Query = _dbContext.SaleBillings.Select(
    //   SaleBilling => new GetSaleBillingByIdResponse
    //   {
    //     TODO
    //   }
    // );

    List<GetSaleBillingByIdResponse> SaleBillings = [];

    /// If cursor is bigger than the size of the collection you will get the following error
    /// ArgumentOutOfRangeException "Index was out of range. Must be non-negative and less than the size of the collection"
    SaleBillings = await Query
      .Where(x => x.SaleBillingId > request.Cursor)
      .Take(20)
      .ToListAsync();

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
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

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

    return _mapper.Map<GetSaleBillingByIdResponse>(SaleBilling);
    // TODO
    // return new GetSaleBillingByIdResponse
    // {
    //    TODO
    // };
  }

  public override async Task<CreateSaleBillingResponse> PostAsync(CreateSaleBillingRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} creating new record ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(SaleBilling).Name
    );

    SaleBilling SaleBilling = _mapper.Map<SaleBilling>(request);
    SaleBilling.CreatedBy = UserId;

    // TODO
    // var SaleBilling = new SaleBilling
    // {
    //   SaleFk = request.SaleFk,
    //   Comments = request.Comments,
    //   TotalDiscount = request.TotalDiscount,
    //   Payment = request.Payment,
    //   CreatedBy = UserId,
    // };

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
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );
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
    // if (request.Id <= 0)
    //   throw new RpcException(new Status(StatusCode.InvalidArgument, "You must supply a valid id"));

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
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );
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