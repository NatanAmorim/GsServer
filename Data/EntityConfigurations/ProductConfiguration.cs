using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class ProductConfiguration : IEntityTypeConfiguration<ProductModel>
{
  public void Configure(EntityTypeBuilder<ProductModel> typeBuilder)
  {

  }
}