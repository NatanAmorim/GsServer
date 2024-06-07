using System.Security.Claims;
using Grpc.Core;
using GsServer.Models;
using GsServer.Protobufs;
using Microsoft.AspNetCore.Authorization;

namespace GsServer.Services;

[Authorize]
public class CustomerRpcService : CustomerService.CustomerServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<CustomerRpcService> _logger;
  public CustomerRpcService(
      ILogger<CustomerRpcService> logger,
      DatabaseContext dbContext
    )
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override async Task<GetPaginatedCustomersResponse> GetPaginatedAsync(GetPaginatedCustomersRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing multiple records ({RecordType}) with cursor {Cursor}",
      RequestTracerId,
      UserId,
      typeof(Customer).Name,
      request.Cursor
    );

    IQueryable<GetCustomerByIdResponse> Query;

    if (request.Cursor is null || request.Cursor == string.Empty)
    {
      Query = _dbContext.Customers
        .Include(c => c.Person)
        .Include(c => c.Dependents)
        .Select(Customer => Customer.ToGetById());
    }
    else
    {
      Query = _dbContext.Customers
        .Include(c => c.Person)
        .Include(c => c.Dependents)
        .Where(x => x.CustomerId.CompareTo(Ulid.Parse(request.Cursor)) > 0)
        .Select(Customer => Customer.ToGetById());
    }

    List<GetCustomerByIdResponse> Customers =
      await Query
        .Take(20)
        .ToListAsync();

    GetPaginatedCustomersResponse response = new();

    response.Customers.AddRange(Customers);
    response.NextCursor = Customers.LastOrDefault()?.CustomerId;

    _logger.LogInformation(
      "({TraceIdentifier}) multiple records ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(Customer).Name
    );

    return response;
  }

  public override async Task<GetCustomerByIdResponse> GetByIdAsync(GetCustomerByIdRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Customer).Name,
      request.CustomerId
    );

    Customer? Customer = await _dbContext.Customers.FindAsync(request.CustomerId);

    if (Customer is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) record ({RecordType}) not found",
        RequestTracerId,
        typeof(Customer).Name
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Nenhum produto com ID {request.CustomerId}"
      ));
    }

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(Customer).Name
    );

    return Customer.ToGetById();

  }

  public override async Task<VoidValue> PostAsync(CreateCustomerRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} creating new record ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(Customer).Name
    );

    Customer Customer = Customer.FromProtoRequest(request, Ulid.Parse(UserId));

    await _dbContext.AddAsync(Customer);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) created successfully, RecordId {RecordId}",
      RequestTracerId,
      typeof(Customer).Name,
      Customer.CustomerId
    );

    return new VoidValue();
  }

  public override Task<VoidValue> PutAsync(UpdateCustomerRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} updating record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Customer).Name,
      request.CustomerId
    );

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) updated successfully",
      RequestTracerId,
      typeof(Customer).Name
    );

    throw new NotImplementedException();

    // TODO
    // CustomerModel? Customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == request.Id);
    // if (Customer is null)
    // {
    //   throw new RpcException(new Status(
    //     StatusCode.NotFound, $"registro não encontrado"
    //   ));
    // }

    // Customer.Name = request.Name;
    // // TODO Add Another fields

    // await _dbContext.SaveChangesAsync();
    // // TODO Log => Record (record type) ID Y was updated. Old value of (field name): (old value). New value: (new value). (This logs specific changes made to a field within a record)
    // return new UpdateCustomerResponse();
  }

  public override async Task<VoidValue> DeleteAsync(DeleteCustomerRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
        "({TraceIdentifier}) User {UserID} deleting record ({RecordType}) with ID ({RecordId})",
        RequestTracerId,
        UserId,
        typeof(Customer).Name,
        request.CustomerId
      );

    Customer? Customer = await _dbContext.Customers.FindAsync(request.CustomerId);

    if (Customer is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) Error deleting record ({RecordType}) with ID {Id}, record not found",
        RequestTracerId,
        typeof(Customer).Name,
        request.CustomerId
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Erro ao remover registro, nenhum registro com ID {request.CustomerId}"
      ));
    }

    /// TODO check if record is being used before deleting it use something like PK or FK

    _dbContext.Customers.Remove(Customer);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
          "({TraceIdentifier}) record ({RecordType}) deleted successfully",
          RequestTracerId,
          typeof(Customer).Name
        );

    return new VoidValue();
  }
}
