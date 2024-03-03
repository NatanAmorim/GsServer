using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class CustomerConfiguration : IEntityTypeConfiguration<CustomerModel>
{
  public void Configure(EntityTypeBuilder<CustomerModel> typeBuilder)
  {

  }
}