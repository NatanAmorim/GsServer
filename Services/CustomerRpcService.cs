using System.Security.Claims;
using AutoMapper;
using Grpc.Core;
using GsServer.Models;
using GsServer.Protobufs;

namespace GsServer.Services;

public class CustomerRpcService : CustomerService.CustomerServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<CustomerRpcService> _logger;
  private readonly IMapper _mapper;
  public CustomerRpcService(
      ILogger<CustomerRpcService> logger,
      DatabaseContext dbContext,
      IMapper mapper
    )
  {
    _logger = logger;
    _dbContext = dbContext;
    _mapper = mapper;
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

    IQueryable<GetCustomerByIdResponse> Query = _dbContext.Customers.Select(
      Customer => _mapper.Map<GetCustomerByIdResponse>(Customer)
    );

    // TODO
    // IQueryable < GetCustomerByIdResponse > Query = _dbContext.Customers.Select(
    //   Customer => new GetCustomerByIdResponse
    //   {
    //     CustomerId = Customer.CustomerId,
    //     Person = new()
    //     {
    //       Name = Customer.Person.Name,
    //       MobilePhoneNumber = Customer.Person.MobilePhoneNumber,
    //       BirthDate = Customer.Person.BirthDate,
    //       Cpf = Customer.Person.Cpf,
    //       // Cin = Customer.Person.Cin,
    //     },
    //     Dependents = {
    //        Customer.Dependents.Select(
    //           Dependent => new Protobufs.Person
    //           {
    //             Name = Dependent.Name,
    //             BirthDate = Dependent.BirthDate,
    //           }
    //        ).ToList(),
    //     },
    //     BillingAddress = Customer.BillingAddress,
    //     AdditionalInformation = Customer.AdditionalInformation,
    //   }
    // );

    List<GetCustomerByIdResponse> Customers = [];

    /// If cursor is bigger than the size of the collection you will get the following error
    /// ArgumentOutOfRangeException "Index was out of range. Must be non-negative and less than the size of the collection"
    Customers = await Query
      .Where(x => x.CustomerId.CompareTo(Ulid.Parse(request.Cursor)) > 0)
      .Take(20)
      .ToListAsync();

    GetPaginatedCustomersResponse response = new();

    response.Customers.AddRange(Customers);
    if (Customers.Count < 20)
    {
      /// Avoiding `ArgumentOutOfRangeException`, basically, don't fetch if null
      response.NextCursor = null;
    }
    else
    {
      /// Id of the last element of the list, same value as `Users[Users.Count - 1].Id`
      response.NextCursor = Customers[^1].CustomerId;
    }

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

    return _mapper.Map<GetCustomerByIdResponse>(Customer);
    // TODO
    // return new GetCustomerByIdResponse
    // {
    //   CustomerId = Customer.CustomerId,
    //   Person = new()
    //   {
    //     Name = Customer.Person.Name,
    //     MobilePhoneNumber = Customer.Person.MobilePhoneNumber,
    //     BirthDate = Customer.Person.BirthDate,
    //     Cpf = Customer.Person.Cpf,
    //     // Cin = Customer.Person.Cin,
    //   },
    //   Dependents = {
    //        Customer.Dependents.Select(
    //           Dependent => new Protobufs.Person
    //           {
    //             Name = Dependent.Name,
    //             BirthDate = Dependent.BirthDate,
    //           }
    //        ).ToList(),
    //     },
    //   BillingAddress = Customer.BillingAddress,
    //   AdditionalInformation = Customer.AdditionalInformation,
    // };
  }

  public override async Task<CreateCustomerResponse> PostAsync(CreateCustomerRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} creating new record ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(Customer).Name
    );

    Customer Customer = _mapper.Map<Customer>(request);
    Customer.CreatedBy = Ulid.Parse(UserId);

    // TODO
    // var Customer = new Customer
    // {
    //   Person = new()
    //   {
    //     Name = request.Person.Name,
    //     MobilePhoneNumber = request.Person.MobilePhoneNumber,
    //     BirthDate = request.Person.BirthDate,
    //     Cpf = request.Person.Cpf,
    //     Cin = request.Person.Cin,
    //     CreatedBy = UserId,
    //   },
    //   Dependents =
    //     request.Dependents.Select(
    //       Dependent => new Models.Person
    //       {
    //         Name = Dependent.Name,
    //         BirthDate = Dependent.BirthDate,
    //         MobilePhoneNumber = Dependent.MobilePhoneNumber,
    //         Cpf = Dependent.Cpf,
    //         // Cin = Dependent.Cin,
    //         CreatedBy = UserId,
    //       }
    //     ).ToList(),
    //   BillingAddress = request.BillingAddress,
    //   AdditionalInformation = request.AdditionalInformation,
    //   CreatedBy = UserId,
    // };

    await _dbContext.AddAsync(Customer);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) created successfully, RecordId {RecordId}",
      RequestTracerId,
      typeof(Customer).Name,
      Customer.CustomerId
    );

    return new CreateCustomerResponse();
  }

  public override Task<UpdateCustomerResponse> PutAsync(UpdateCustomerRequest request, ServerCallContext context)
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
    // if (request.Id <= 0)
    //   throw new RpcException(new Status(StatusCode.InvalidArgument, "You must supply a valid id"));

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

  public override async Task<DeleteCustomerResponse> DeleteAsync(DeleteCustomerRequest request, ServerCallContext context)
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

    return new DeleteCustomerResponse();
  }
}
