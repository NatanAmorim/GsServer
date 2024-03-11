// using System.Security.Claims;
// using Grpc.Core;
// using gs_server.Models;
// using gs_server.Protobufs;

// namespace gs_server.Services;

// public class ProductRpcService : ProductService.ProductServiceBase
// {
//   private readonly DatabaseContext _dbContext;
//   private readonly ILogger<ProductRpcService> _logger;
//   public ProductRpcService(ILogger<ProductRpcService> logger, DatabaseContext dbContext)
//   {
//     _logger = logger;
//     _dbContext = dbContext;
//   }

//   public override async Task<GetAllProductsResponse> GetAll(GetAllProductsRequest request, ServerCallContext context)
//   {
//     _logger.LogInformation("Listing Products");
//     List<ProductModel> Products =
//           await _dbContext.Products
//           .ToListAsync();

//     GetAllProductsResponse response = new();

//     response.Products.AddRange(
//       Products.Select(
//         Product => new GetProductByIdResponse
//         {
//           ProductId = Product.ProductId,
//           Name = Product.Name,
//           PicturePath = Product.PicturePath,
//           Variants =
//           {
//             Product.Variants.Select(
//               Variant => new ProductVariant
//               {
//                 VariantId = Variant.VariantId,
//                 Description = Variant.Description,
//                 BarCode = Variant.BarCode,
//                 UnitPrice = Variant.UnitPrice,
//                 StockAmount = Variant.StockAmount,
//                 StockMinimumAmount = Variant.StockMinimumAmount,
//               }
//             ),
//           },
//         }
//       ).ToList()
//     );

//     _logger.LogInformation("Products have been listed successfully");
//     return response;
//   }

//   public override async Task<GetProductByIdResponse> GetById(GetProductByIdRequest request, ServerCallContext context)
//   {
//     _logger.LogInformation(
//       "Searching for Product with ID {Id}",
//       request.ProductId
//     );

//     ProductModel? Product = await _dbContext.Products.FindAsync(request.ProductId);

//     if (Product is null)
//     {
//       _logger.LogWarning(
//         "Error search Product request, no Product with ID {Id}",
//         request.ProductId
//       );
//       throw new RpcException(new Status(
//         StatusCode.NotFound, $"Erro ao procurar produto, nenhum produto com ID {request.ProductId}"
//       ));
//     }

//     _logger.LogInformation(
//       "Product with ID {Id} found successfully",
//       request.ProductId
//     );
//     return new GetProductByIdResponse
//     {
//       ProductId = Product.ProductId,
//       Name = Product.Name,
//       PicturePath = Product.PicturePath,
//       Variants =
//       {
//         Product.Variants.Select(
//           Variant => new ProductVariant
//           {
//             VariantId = Variant.VariantId,
//             Description = Variant.Description,
//             BarCode = Variant.BarCode,
//             UnitPrice = Variant.UnitPrice,
//             StockAmount = Variant.StockAmount,
//             StockMinimumAmount = Variant.StockMinimumAmount,
//           }
//         ),
//       },
//     };
//   }

//   public override async Task<CreateProductResponse> Post(CreateProductRequest request, ServerCallContext context)
//   {
//     int UserId = int.Parse(
//       context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
//     );

//     // TODO upload binary from request.PicturePath to aws s3 bucket and get PicturePath back
//     string? PicturePath = null;

//     var Product = new ProductModel
//     {
//       Name = request.Name,
//       PicturePath = PicturePath,
//       Variants = request.Variants.Select(
//           Variant => new ProductVariantModel
//           {
//             Description = Variant.Description.ToString(),
//             BarCode = Variant.BarCode.ToString(),
//             UnitPrice = Variant.UnitPrice,
//             StockAmount = Variant.StockAmount,
//             StockMinimumAmount = Variant.StockMinimumAmount,
//           }
//       ).ToList(),
//       CreatedBy = UserId,
//     };

//     await _dbContext.AddAsync(Product);
//     await _dbContext.SaveChangesAsync();

//     return new CreateProductResponse();
//   }

//   public override Task<UpdateProductResponse> Put(UpdateProductRequest request, ServerCallContext context)
//   {
//     throw new NotImplementedException();

//     // TODO
//     // if (request.Id <= 0)
//     //   throw new RpcException(new Status(StatusCode.InvalidArgument, "You must supply a valid id"));

//     // ProductModel? Product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == request.Id);
//     // if (Product is null)
//     // {
//     //   throw new RpcException(new Status(
//     //     StatusCode.NotFound, $"Produto não encontrado"
//     //   ));
//     // }

//     // Product.Name = request.Name;
//     // // TODO Add Another fields

//     // await _dbContext.SaveChangesAsync();
//     // return new UpdateProductResponse();
//   }

//   public override async Task<DeleteProductResponse> Delete(DeleteProductRequest request, ServerCallContext context)
//   {
//     _logger.LogInformation("Deleting Product with ID {Id}", request.ProductId);
//     ProductModel? Product = await _dbContext.Products.FindAsync(request.ProductId);

//     if (Product is null)
//     {
//       _logger.LogWarning(
//         "Error in delete Product request, no Product with ID {Id}",
//         request.ProductId
//       );
//       throw new RpcException(new Status(
//         StatusCode.NotFound, $"Erro ao remover produto, nenhum produto com ID {request.ProductId}"
//       ));
//     }

//     _dbContext.Products.Remove(Product);
//     await _dbContext.SaveChangesAsync();

//     _logger.LogInformation(
//       "Product deleted successfully ID {Id}",
//       request.ProductId
//     );

//     return new DeleteProductResponse();
//   }
// }
