using GsServer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GsServer.EntityConfigurations;

public class CustomerConfiguration : IEntityTypeConfiguration<CustomerModel>
{
  public void Configure(EntityTypeBuilder<CustomerModel> typeBuilder)
  {

  }
}