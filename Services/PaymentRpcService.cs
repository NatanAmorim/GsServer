using System.Security.Claims;
using Grpc.Core;
using GsServer.Models;
using GsServer.Protobufs;
using Microsoft.AspNetCore.Authorization;

namespace GsServer.Services;

[Authorize]
public class PaymentRpcService : PaymentService.PaymentServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<PaymentRpcService> _logger;
  public PaymentRpcService(
      ILogger<PaymentRpcService> logger,
      DatabaseContext dbContext
    )
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override async Task<GetPaginatedPaymentsResponse> GetPaginatedAsync(GetPaginatedPaymentsRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing multiple records ({RecordType}) with cursor {Cursor}",
      RequestTracerId,
      UserId,
      typeof(Payment).Name,
      request.Cursor
    );

    IQueryable<GetPaymentByIdResponse> Query = _dbContext.Payments.Select(
      Payment => Payment.ToGetById()
    );

    List<GetPaymentByIdResponse> Payments = [];

    if (request.Cursor is null)
    {
      Payments = await Query
        .Take(20)
        .ToListAsync();
    }
    else
    {
      /// If cursor is bigger than the size of the collection you will get the following error
      /// ArgumentOutOfRangeException "Index was out of range. Must be non-negative and less than the size of the collection"
      Payments = await Query
        .Where(x => x.PaymentId.CompareTo(Ulid.Parse(request.Cursor)) > 0)
        .Take(20)
        .ToListAsync();
    }

    GetPaginatedPaymentsResponse response = new();

    response.Payments.AddRange(Payments);
    if (Payments.Count < 20)
    {
      /// Avoiding `ArgumentOutOfRangeException`, basically, don't fetch if null
      response.NextCursor = null;
    }
    else
    {
      /// Id of the last element of the list, same value as `Users[Users.Count - 1].Id`
      response.NextCursor = Payments[^1].PaymentId;
    }

    _logger.LogInformation(
      "({TraceIdentifier}) multiple records ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(Payment).Name
    );
    return response;
  }

  public override async Task<GetPaymentByIdResponse> GetByIdAsync(GetPaymentByIdRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Payment).Name,
      request.PaymentId
    );

    Payment? Payment = await _dbContext.Payments.FindAsync(request.PaymentId);

    if (Payment is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) record ({RecordType}) not found",
        RequestTracerId,
        typeof(Payment).Name
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Nenhum produto com ID {request.PaymentId}"
      ));
    }

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(Payment).Name
    );

    return Payment.ToGetById();
  }

  public override async Task<CreatePaymentResponse> PostAsync(CreatePaymentRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} creating new record ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(Payment).Name
    );

    Payment Payment = Payment.FromProtoRequest(request, Ulid.Parse(UserId));

    await _dbContext.AddAsync(Payment);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) created successfully, RecordId {RecordId}",
      RequestTracerId,
      typeof(Payment).Name,
      Payment.PaymentId
    );

    return new CreatePaymentResponse();
  }

  public override Task<UpdatePaymentResponse> PutAsync(UpdatePaymentRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} updating record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Payment).Name,
      request.PaymentId
    );

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) updated successfully",
      RequestTracerId,
      typeof(Payment).Name
    );

    throw new NotImplementedException();

    // TODO
    // PaymentModel? Payment = await _dbContext.Payments.FirstOrDefaultAsync(x => x.Id == request.Id);
    // if (Payment is null)
    // {
    //   throw new RpcException(new Status(
    //     StatusCode.NotFound, $"registro não encontrado"
    //   ));
    // }

    // Payment.Name = request.Name;
    // // TODO Add Another fields

    // await _dbContext.SaveChangesAsync();
    // // TODO Log => Record (record type) ID Y was updated. Old value of (field name): (old value). New value: (new value). (This logs specific changes made to a field within a record)
    // return new UpdatePaymentResponse();
  }

  public override async Task<DeletePaymentResponse> DeleteAsync(DeletePaymentRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
        "({TraceIdentifier}) User {UserID} deleting record ({RecordType}) with ID ({RecordId})",
        RequestTracerId,
        UserId,
        typeof(Payment).Name,
        request.PaymentId
      );

    Payment? Payment = await _dbContext.Payments.FindAsync(request.PaymentId);

    if (Payment is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) Error deleting record ({RecordType}) with ID {Id}, record not found",
        RequestTracerId,
        typeof(Payment).Name,
        request.PaymentId
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Erro ao remover registro, nenhum registro com ID {request.PaymentId}"
      ));
    }

    /// TODO check if record is being used before deleting it use something like PK or FK

    _dbContext.Payments.Remove(Payment);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
          "({TraceIdentifier}) record ({RecordType}) deleted successfully",
          RequestTracerId,
          typeof(Payment).Name
        );

    return new DeletePaymentResponse();
  }
}
