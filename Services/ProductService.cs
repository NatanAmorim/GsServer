using Grpc.Core;

namespace gs_server.Services;

public class ProductRpcService : ProductService.ProductServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<ProductRpcService> _logger;
  public ProductRpcService(ILogger<ProductRpcService> logger, DatabaseContext dbContext)
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override Task<GetAllProductsResponse> GetAll(GetAllProductsRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<GetProductByIdResponse> GetById(GetProductByIdRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<CreateProductResponse> Post(CreateProductRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<UpdateProductResponse> Put(UpdateProductRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<DeleteProductResponse> Delete(DeleteProductRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }
}
