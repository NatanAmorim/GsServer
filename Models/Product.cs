using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GsServer.Protobufs;

namespace GsServer.Models;

public class Product
{
  [Key]
  public Ulid ProductId { get; init; } = Ulid.NewUlid();
  [MinLength(4, ErrorMessage = "O nome deve ter no mínimo 4 caracteres")]
  [MaxLength(32, ErrorMessage = "O nome deve ter no máximo 32 caracteres")]
  [Required(ErrorMessage = "Campo de preenchimento obrigatório", AllowEmptyStrings = false)]
  public required string Name { get; set; }
  // Image path on a Cloud Storage (Like: Imgur, S3, Azure blob).
  // All images will be scaled to 128px(w) x 128px(h).
  public string? PicturePath { get; set; }
  [Required]
  [ForeignKey(nameof(ProductBrandId))]
  public required Ulid ProductBrandId { get; set; }
  public virtual ProductBrand ProductBrand { get; set; } = null!;
  // (e.g., Hats, Shirts, Pants, Shorts, Shoes, Dresses).
  [Required]
  [ForeignKey(nameof(ProductCategoryId))]
  public required Ulid ProductCategoryId { get; set; }
  public virtual ProductCategory ProductCategory { get; set; } = null!;
  [Required]
  public required ICollection<ProductVariant> Variants { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required]
  public required Ulid CreatedBy { get; set; }

  public static Product FromProtoRequest(CreateProductRequest request, string? picturePath, Ulid createdBy)
    => new()
    {
      Name = request.Name,
      PicturePath = picturePath,
      ProductBrandId = Ulid.Parse(request.ProductBrandId),
      ProductCategoryId = Ulid.Parse(request.ProductCategoryId),
      Variants = request.Variants.Select(
        Variant => new ProductVariant
        {
          ProductVariantId = Ulid.Parse(Variant.ProductVariantId),
          Color = Variant.Color,
          Size = Variant.Size,
          BarCode = Variant.BarCode,
          Sku = Variant.Sku,
          UnitPrice = Variant.UnitPrice,
          Inventory = new ProductVariantInventory
          {
            ProductVariantInventoryId = Ulid.Parse(Variant.Inventory.ProductVariantInventoryId),
            QuantityAvailable = Variant.Inventory.QuantityAvailable,
            MinimumStockAmount = Variant.Inventory.MinimumStockAmount,
          }
        }
      ).ToList(),
      CreatedBy = createdBy,
    };

  public GetProductByIdResponse ToGetById()
    => new()
    {
      Name = Name,
      PicturePath = PicturePath,
      ProductBrand = new Protobufs.ProductBrand()
      {
        ProductBrandId = ProductBrand.ProductBrandId.ToString(),
        Name = ProductBrand.Name,
      },
      ProductCategory = new Protobufs.ProductCategory()
      {
        ProductCategoryId = ProductCategory.ProductCategoryId.ToString(),
        Name = ProductCategory.Name,
      },
      Variants =
        {
          Variants.Select(
            Variant => new Protobufs.ProductVariant
            {
              ProductVariantId = Variant.ProductVariantId.ToString(),
              Color = Variant.Color,
              Size = Variant.Size,
              BarCode = Variant.BarCode,
              Sku = Variant.Sku,
              UnitPrice = Variant.UnitPrice,
              Inventory = new Protobufs.ProductVariantInventory
              {
                ProductVariantInventoryId = Variant.Inventory.ProductVariantInventoryId.ToString(),
                QuantityAvailable = Variant.Inventory.QuantityAvailable,
                MinimumStockAmount = Variant.Inventory.MinimumStockAmount,
              }
            }
          ),
        },
    };
}
