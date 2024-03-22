using GsServer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GsServer.EntityConfigurations;

public class ProductConfiguration : IEntityTypeConfiguration<ProductModel>
{
  public void Configure(EntityTypeBuilder<ProductModel> typeBuilder)
  {

  }
}