using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
  public void Configure(EntityTypeBuilder<Sale> typeBuilder)
  {
    typeBuilder.Property(x => x.Description).HasColumnType("varchar(80)");

  }
}