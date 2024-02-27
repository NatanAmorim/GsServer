using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
  public void Configure(EntityTypeBuilder<Address> typeBuilder)
  {
    typeBuilder.Property(x => x.StreetAddress).HasColumnType("varchar(20)");
    typeBuilder.Property(x => x.PostalCode).HasColumnType("varchar(15)");
  }
}