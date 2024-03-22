using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class SaleConfiguration : IEntityTypeConfiguration<SaleModel>
{
  public void Configure(EntityTypeBuilder<SaleModel> typeBuilder)
  {
    typeBuilder.Property(x => x.Comments).HasColumnType("varchar(80)");

  }
}