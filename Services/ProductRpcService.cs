using System.Security.Claims;
using Grpc.Core;
using GsServer.Models;
using GsServer.Protobufs;
using Microsoft.AspNetCore.Authorization;

namespace GsServer.Services;

[Authorize]
public class ProductRpcService : ProductService.ProductServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<ProductRpcService> _logger;
  public ProductRpcService(
    ILogger<ProductRpcService> logger,
    DatabaseContext dbContext
  )
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override async Task<GetAllProductsResponse> GetAllAsync(VoidValue request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing all records ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(Product).Name
    );

    IQueryable<GetProductByIdResponse> Query = _dbContext.Products.Select(
      Product => Product.ToGetById()
    );

    List<GetProductByIdResponse> Products = await Query.ToListAsync();

    GetAllProductsResponse response = new();

    response.Products.AddRange(Products);

    _logger.LogInformation(
      "({TraceIdentifier}) all records ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(Product).Name
    );

    return response;
  }

  public override async Task<GetProductByIdResponse> GetByIdAsync(GetProductByIdRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Product).Name,
      request.ProductId
    );

    Product? Product = await _dbContext.Products.FindAsync(request.ProductId);

    if (Product is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) record ({RecordType}) not found",
        RequestTracerId,
        typeof(Product).Name
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Nenhum produto com ID {request.ProductId}"
      ));
    }

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(Product).Name
    );

    return Product.ToGetById();
  }

  public override async Task<VoidValue> PostAsync(CreateProductRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} creating new record ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(Product).Name
    );

    // TODO upload binary from request.PicturePath to aws s3 bucket and get PicturePath back
    string? PicturePath = null;

    Product Product = Product.FromProtoRequest(request, PicturePath, Ulid.Parse(UserId));

    await _dbContext.AddAsync(Product);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) created successfully, RecordId {RecordId}",
      RequestTracerId,
      typeof(Product).Name,
      Product.ProductId
    );

    return new VoidValue();
  }

  public override Task<VoidValue> PutAsync(UpdateProductRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} updating record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Product).Name,
      request.ProductId
    );

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) updated successfully",
      RequestTracerId,
      typeof(Product).Name
    );

    throw new NotImplementedException();

    // TODO
    // ProductModel? Product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == request.Id);
    // if (Product is null)
    // {
    //   throw new RpcException(new Status(
    //     StatusCode.NotFound, $"Produto não encontrado"
    //   ));
    // }

    // Product.Name = request.Name;
    // // TODO Add Another fields

    // await _dbContext.SaveChangesAsync();
    // // TODO Log => Record (record type) ID Y was updated. Old value of (field name): (old value). New value: (new value). (This logs specific changes made to a field within a record)
    // return new UpdateProductResponse();

  }

  public override async Task<VoidValue> DeleteAsync(DeleteProductRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
        "({TraceIdentifier}) User {UserID} deleting record ({RecordType}) with ID ({RecordId})",
        RequestTracerId,
        UserId,
        typeof(Product).Name,
        request.ProductId
      );

    Product? Product = await _dbContext.Products.FindAsync(request.ProductId);

    if (Product is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) Error deleting record ({RecordType}) with ID {Id}, record not found",
        RequestTracerId,
        typeof(Product).Name,
        request.ProductId
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Erro ao remover produto, nenhum produto com ID {request.ProductId}"
      ));
    }

    /// TODO check if product is being used before deleting it use something like PK or FK

    _dbContext.Products.Remove(Product);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
          "({TraceIdentifier}) record ({RecordType}) deleted successfully",
          RequestTracerId,
          typeof(Product).Name
        );

    return new VoidValue();
  }

  public override Task<GetAllProductBrandsResponse> GetAllBrandsAsync(VoidValue request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    throw new NotImplementedException();
  }

  public override Task<VoidValue> PostBrandAsync(CreateProductBrandRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    throw new NotImplementedException();
  }

  public override Task<GetAllProductCategoriesResponse> GetAllCategoriesAsync(VoidValue request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    throw new NotImplementedException();
  }

  public override Task<VoidValue> PostCategoryAsync(CreateProductCategoryRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    throw new NotImplementedException();
  }
}
