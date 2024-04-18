using System.Security.Claims;
using AutoMapper;
using Grpc.Core;
using GsServer.Models;
using GsServer.Protobufs;

namespace GsServer.Services;

public class SubscriptionBillingRpcService : SubscriptionBillingService.SubscriptionBillingServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<SubscriptionBillingRpcService> _logger;
  private readonly IMapper _mapper;
  public SubscriptionBillingRpcService(
      ILogger<SubscriptionBillingRpcService> logger,
      DatabaseContext dbContext,
      IMapper mapper

    )
  {
    _logger = logger;
    _dbContext = dbContext;
    _mapper = mapper;
  }

  public override async Task<GetPaginatedSubscriptionBillingsResponse> GetPaginatedAsync(GetPaginatedSubscriptionBillingsRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing multiple records ({RecordType}) with cursor {Cursor}",
      RequestTracerId,
      UserId,
      typeof(SubscriptionBilling).Name,
      request.Cursor
    );

    IQueryable<GetSubscriptionBillingByIdResponse> Query = _dbContext.SubscriptionBillings.Select(
      SubscriptionBilling => _mapper.Map<GetSubscriptionBillingByIdResponse>(SubscriptionBilling)
    );

    // TODO
    // IQueryable<GetSubscriptionBillingByIdResponse> Query = _dbContext.SubscriptionBillings.Select(
    //   SubscriptionBilling => new GetSubscriptionBillingByIdResponse
    //   {
    //     TODO
    //   }
    // );

    List<GetSubscriptionBillingByIdResponse> SubscriptionBillings = [];

    /// If cursor is bigger than the size of the collection you will get the following error
    /// ArgumentOutOfRangeException "Index was out of range. Must be non-negative and less than the size of the collection"
    SubscriptionBillings = await Query
      .Where(x => x.SubscriptionBillingId > request.Cursor)
      .Take(20)
      .ToListAsync();

    GetPaginatedSubscriptionBillingsResponse response = new();

    response.SubscriptionBillings.AddRange(SubscriptionBillings);
    if (SubscriptionBillings.Count < 20)
    {
      /// Avoiding `ArgumentOutOfRangeException`, basically, don't fetch if null
      response.NextCursor = null;
    }
    else
    {
      /// Id of the last element of the list, same value as `Users[Users.Count - 1].Id`
      response.NextCursor = SubscriptionBillings[^1].SubscriptionBillingId;
    }

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
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

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

    return _mapper.Map<GetSubscriptionBillingByIdResponse>(SubscriptionBilling);

    // TODO
    // return new GetSubscriptionBillingByIdResponse
    // {
    //   todo
    // };
  }

  public override async Task<CreateSubscriptionBillingResponse> PostAsync(CreateSubscriptionBillingRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} creating new record ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(SubscriptionBilling).Name
    );

    SubscriptionBilling SubscriptionBilling = _mapper.Map<SubscriptionBilling>(request);
    SubscriptionBilling.CreatedBy = UserId;

    // TODO
    // var SubscriptionBilling = new SubscriptionBilling
    // {
    //   SubscriptionFk = request.SubscriptionFk,
    //   Comments = request.Comments,
    //   TotalDiscount = request.TotalDiscount,
    //   Payment = new Payment
    //   {
    //     Comments = request.Payment.Comments,
    //     Installments = request.Payment.Installments.Select(
    //       Installment => new Models.PaymentInstallment
    //       {
    //         PaymentInstallmentPk = Installment.PaymentInstallmentPk,
    //         PaymentFk = Installment.PaymentFk,
    //         InstallmentNumber = Installment.PaymentFk,
    //         InstallmentAmount = Installment.InstallmentAmount,
    //         PaymentMethod = Installment.PaymentMethod,
    //         DueDate = new(
    //             Installment.DueDate.Year,
    //             Installment.DueDate.Month,
    //             Installment.DueDate.Day
    //           ),
    //       }
    //     ).ToList(),
    //     CreatedBy = UserId,
    //   },
    //   CreatedBy = UserId,
    // };

    await _dbContext.AddAsync(SubscriptionBilling);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) created successfully, RecordId {RecordId}",
      RequestTracerId,
      typeof(SubscriptionBilling).Name,
      SubscriptionBilling.SubscriptionBillingId
    );

    return new CreateSubscriptionBillingResponse();
  }

  public override Task<UpdateSubscriptionBillingResponse> PutAsync(UpdateSubscriptionBillingRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );
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
    // if (request.Id <= 0)
    //   throw new RpcException(new Status(StatusCode.InvalidArgument, "You must supply a valid id"));

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

  public override async Task<DeleteSubscriptionBillingResponse> DeleteAsync(DeleteSubscriptionBillingRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );
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

    return new DeleteSubscriptionBillingResponse();
  }
}
