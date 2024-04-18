using System.Security.Claims;
using AutoMapper;
using Grpc.Core;
using GsServer.Models;
using GsServer.Protobufs;

namespace GsServer.Services;

public class ProductRpcService : ProductService.ProductServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<ProductRpcService> _logger;
  private readonly IMapper _mapper;
  public ProductRpcService(
    ILogger<ProductRpcService> logger,
    DatabaseContext dbContext,
    IMapper mapper
  )
  {
    _logger = logger;
    _dbContext = dbContext;
    _mapper = mapper;
  }

  public override async Task<GetAllProductsResponse> GetAllAsync(GetAllProductsRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing all records ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(Product).Name
    );

    IQueryable<GetProductByIdResponse> Query = _dbContext.Products.Select(
      Product => _mapper.Map<GetProductByIdResponse>(Product)
    );

    // TODO
    // response.Products.AddRange(
    //   Products.Select(
    //     Product => new GetProductByIdResponse
    //     {
    //       Name = Product.Name,
    //       Description = Product.Description,
    //       PicturePath = Product.PicturePath,
    //       ProductBrandFk = Product.ProductBrand,
    //       ProductCategoryFk = Product.ProductCategory,
    //       Variants =
    //       {
    //         Product.Variants.Select(
    //           Variant => new Protobufs.ProductVariant
    //           {
    //             ProductVariantPk = Variant.ProductVariantPk,
    //             Color = Variant.Color,
    //             Size = Variant.Size,
    //             BarCode = Variant.BarCode,
    //             Sku = Variant.Sku,
    //             UnitPrice = Variant.UnitPrice,
    //             Inventory = new ProductVariantInventory
    //             {
    //               ProductVariantInventoryPk = Variant.Inventory.ProductVariantInventoryPk,
    //               ProductVariantFk = Variant.Inventory.ProductVariantFk,
    //               QuantityAvailable = Variant.Inventory.QuantityAvailable,
    //               MinimumStockAmount = Variant.Inventory.MinimumStockAmount,
    //             }
    //           }
    //         ).ToList(),
    //       },
    //     }
    //   ).ToList()
    // );

    List<GetProductByIdResponse> Products = [];

    /// If cursor is bigger than the size of the collection you will get the following error
    /// ArgumentOutOfRangeException "Index was out of range. Must be non-negative and less than the size of the collection"
    Products = await Query
      .ToListAsync();

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
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

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

    return _mapper.Map<GetProductByIdResponse>(Product);

    // TODO
    // return new GetProductByIdResponse
    // {
    //   Name = Product.Name,
    //   Description = Product.Description,
    //   PicturePath = Product.PicturePath,
    //   ProductBrandFk = Product.ProductBrand,
    //   ProductCategoryFk = Product.ProductCategory,
    //   Variants =
    //   {
    //     Product.Variants.Select(
    //       Variant => new ProductVariant
    //       {
    //         ProductVariantPk = Variant.ProductVariantPk,
    //         Color = Variant.Color,
    //         Size = Variant.Size,
    //         BarCode = Variant.BarCode,
    //         Sku = Variant.Sku,
    //         UnitPrice = Variant.UnitPrice,
    //         Inventory = new Protobufs.ProductVariantInventory
    //         {
    //           ProductVariantInventoryPk = Variant.Inventory.ProductVariantInventoryPk,
    //           ProductVariantFk = Variant.Inventory.ProductVariantFk,
    //           QuantityAvailable = Variant.Inventory.QuantityAvailable,
    //           MinimumStockAmount = Variant.Inventory.MinimumStockAmount,
    //         }
    //       }
    //     ),
    //   },
    // };
  }

  public override async Task<CreateProductResponse> PostAsync(CreateProductRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} creating new record ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(Product).Name
    );

    // TODO upload binary from request.PicturePath to aws s3 bucket and get PicturePath back
    string? PicturePath = null;

    Product Product = _mapper.Map<Product>(request);

    // var Product = new Product
    // {
    //   Name = request.Name,
    //   Description = request.Description,
    //   PicturePath = PicturePath,
    //   ProductBrand = request.ProductBrand,
    //   ProductCategory = request.ProductCategory,
    //   Variants = request.Variants.Select(
    //       Variant => new Models.ProductVariant
    //       {
    //         ProductVariantPk = Variant.ProductVariantPk,
    //         Color = Variant.Color,
    //         Size = Variant.Size,
    //         BarCode = Variant.BarCode,
    //         Sku = Variant.Sku,
    //         UnitPrice = Variant.UnitPrice,
    //         Inventory = new ProductVariantInventory
    //         {
    //           ProductVariantInventoryPk = Variant.Inventory.ProductVariantInventoryPk,
    //           ProductVariantFk = Variant.Inventory.ProductVariantFk,
    //           QuantityAvailable = Variant.Inventory.QuantityAvailable,
    //           MinimumStockAmount = Variant.Inventory.MinimumStockAmount,
    //         }
    //       }
    //   ).ToList(),
    //   CreatedBy = UserId,
    // };

    await _dbContext.AddAsync(Product);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) created successfully, RecordId {RecordId}",
      RequestTracerId,
      typeof(Product).Name,
      Product.ProductId
    );

    return new CreateProductResponse();
  }

  public override Task<UpdateProductResponse> PutAsync(UpdateProductRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );
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

  public override async Task<DeleteProductResponse> DeleteAsync(DeleteProductRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );
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

    return new DeleteProductResponse();
  }

  public override Task<GetAllProductBrandsResponse> GetAllBrandsAsync(GetAllProductBrandsRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    throw new NotImplementedException();
  }

  public override Task<CreateProductBrandResponse> PostBrandAsync(CreateProductBrandRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    throw new NotImplementedException();
  }

  public override Task<GetAllProductCategoriesResponse> GetAllCategoriesAsync(GetAllProductCategoriesRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    throw new NotImplementedException();
  }

  public override Task<CreateProductCategoryResponse> PostCategoryAsync(CreateProductCategoryRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    throw new NotImplementedException();
  }
}
