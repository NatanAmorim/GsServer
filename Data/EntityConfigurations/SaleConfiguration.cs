using GsServer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GsServer.EntityConfigurations;

public class SaleConfiguration : IEntityTypeConfiguration<SaleModel>
{
  public void Configure(EntityTypeBuilder<SaleModel> typeBuilder)
  {
    typeBuilder.Property(x => x.Comments).HasColumnType("varchar(80)");

  }
}