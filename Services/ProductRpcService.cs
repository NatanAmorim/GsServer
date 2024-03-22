using System.Security.Claims;
using Grpc.Core;
using GsServer.Models;
using GsServer.Protobufs;

namespace GsServer.Services;

public class ProductRpcService : ProductService.ProductServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<ProductRpcService> _logger;
  public ProductRpcService(ILogger<ProductRpcService> logger, DatabaseContext dbContext)
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override async Task<GetAllProductsResponse> GetAll(GetAllProductsRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing all records ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(ProductModel).Name
    );

    List<ProductModel> Products =
          await _dbContext.Products
          .ToListAsync();

    GetAllProductsResponse response = new();

    response.Products.AddRange(
      Products.Select(
        Product => new GetProductByIdResponse
        {
          Name = Product.Name,
          Description = Product.Description,
          PicturePath = Product.PicturePath,
          ProductBrandId = Product.ProductBrandId,
          ProductCategoryId = Product.ProductCategoryId,
          Variants =
          {
            Product.Variants.Select(
              Variant => new ProductVariant
              {
                ProductVariantId = Variant.ProductVariantId,
                Color = Variant.Color,
                Size = Variant.Size,
                BarCode = Variant.BarCode,
                Sku = Variant.Sku,
                UnitPrice = Variant.UnitPrice,
                Inventory = new ProductVariantInventory
                {
                  ProductVariantInventoryId = Variant.Inventory.ProductVariantInventoryId,
                  ProductVariantId = Variant.Inventory.ProductVariantId,
                  QuantityAvailable = Variant.Inventory.QuantityAvailable,
                  MinimumStockAmount = Variant.Inventory.MinimumStockAmount,
                }
              }
            ),
          },
        }
      ).ToList()
    );

    _logger.LogInformation(
      "({TraceIdentifier}) all records ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(ProductModel).Name
    );
    return response;
  }

  public override async Task<GetProductByIdResponse> GetById(GetProductByIdRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(ProductModel).Name,
      request.ProductId
    );

    ProductModel? Product = await _dbContext.Products.FindAsync(request.ProductId);

    if (Product is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) record ({RecordType}) not found",
        RequestTracerId,
        typeof(ProductModel).Name
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Nenhum produto com ID {request.ProductId}"
      ));
    }

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(ProductModel).Name
    );

    return new GetProductByIdResponse
    {
      Name = Product.Name,
      Description = Product.Description,
      PicturePath = Product.PicturePath,
      ProductBrandId = Product.ProductBrandId,
      ProductCategoryId = Product.ProductCategoryId,
      Variants =
      {
        Product.Variants.Select(
          Variant => new ProductVariant
          {
            ProductVariantId = Variant.ProductVariantId,
            Color = Variant.Color,
            Size = Variant.Size,
            BarCode = Variant.BarCode,
            Sku = Variant.Sku,
            UnitPrice = Variant.UnitPrice,
            Inventory = new ProductVariantInventory
            {
              ProductVariantInventoryId = Variant.Inventory.ProductVariantInventoryId,
              ProductVariantId = Variant.Inventory.ProductVariantId,
              QuantityAvailable = Variant.Inventory.QuantityAvailable,
              MinimumStockAmount = Variant.Inventory.MinimumStockAmount,
            }
          }
        ),
      },
    };
  }

  public override async Task<CreateProductResponse> Post(CreateProductRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} creating new record ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(ProductModel).Name
    );

    // TODO upload binary from request.PicturePath to aws s3 bucket and get PicturePath back
    string? PicturePath = null;

    var Product = new ProductModel
    {
      Name = request.Name,
      Description = request.Description,
      PicturePath = PicturePath,
      ProductBrandId = request.ProductBrandId,
      ProductCategoryId = request.ProductCategoryId,
      Variants = request.Variants.Select(
          Variant => new ProductVariantModel
          {
            ProductVariantId = Variant.ProductVariantId,
            Color = Variant.Color,
            Size = Variant.Size,
            BarCode = Variant.BarCode,
            Sku = Variant.Sku,
            UnitPrice = Variant.UnitPrice,
            Inventory = new ProductVariantInventoryModel
            {
              ProductVariantInventoryId = Variant.Inventory.ProductVariantInventoryId,
              ProductVariantId = Variant.Inventory.ProductVariantId,
              QuantityAvailable = Variant.Inventory.QuantityAvailable,
              MinimumStockAmount = Variant.Inventory.MinimumStockAmount,
            }
          }
      ).ToList(),
      CreatedBy = UserId,
    };

    await _dbContext.AddAsync(Product);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) created successfully, RecordId {RecordId}",
      RequestTracerId,
      typeof(ProductModel).Name,
      Product.ProductId
    );

    return new CreateProductResponse();
  }

  public override Task<UpdateProductResponse> Put(UpdateProductRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} updating record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(ProductModel).Name,
      request.ProductId
    );

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) updated successfully",
      RequestTracerId,
      typeof(ProductModel).Name
    );

    throw new NotImplementedException();

    // TODO
    // if (request.Id <= 0)
    //   throw new RpcException(new Status(StatusCode.InvalidArgument, "You must supply a valid id"));

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

  public override async Task<DeleteProductResponse> Delete(DeleteProductRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} deleting record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(ProductModel).Name,
      request.ProductId
    );

    ProductModel? Product = await _dbContext.Products.FindAsync(request.ProductId);

    if (Product is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) Error deleting record ({RecordType}) with ID {Id}, record not found",
        RequestTracerId,
        typeof(ProductModel).Name,
        request.ProductId
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Erro ao remover produto, nenhum produto com ID {request.ProductId}"
      ));
    }

    _dbContext.Products.Remove(Product);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) deleted successfully",
      RequestTracerId,
      typeof(ProductModel).Name
    );

    return new DeleteProductResponse();
  }

  public override Task<GetAllProductBrandsResponse> GetAllBrands(GetAllProductBrandsRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    throw new NotImplementedException();
  }

  public override Task<CreateProductBrandResponse> PostBrand(CreateProductBrandRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    throw new NotImplementedException();
  }

  public override Task<GetAllProductCategoriesResponse> GetAllCategories(GetAllProductCategoriesRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    throw new NotImplementedException();
  }

  public override Task<CreateProductCategoryResponse> PostCategory(CreateProductCategoryRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    throw new NotImplementedException();
  }
}
