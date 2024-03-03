using Grpc.Core;
using gs_server.Protobufs;

namespace gs_server.Services;

public class CustomerRpcService : CustomerService.CustomerServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<CustomerRpcService> _logger;
  public CustomerRpcService(ILogger<CustomerRpcService> logger, DatabaseContext dbContext)
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override Task<GetAllCustomersResponse> GetAll(GetAllCustomersRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<GetCustomerByIdResponse> GetById(GetCustomerByIdRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<GetAllCustomersOptionsResponse> GetAllOptions(GetAllCustomersOptionsRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<CreateCustomerResponse> Post(CreateCustomerRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<UpdateCustomerResponse> Put(UpdateCustomerRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<DeleteCustomerResponse> Delete(DeleteCustomerRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }
}
