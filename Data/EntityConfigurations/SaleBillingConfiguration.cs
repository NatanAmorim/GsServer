using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class SaleBillingConfiguration : IEntityTypeConfiguration<SaleBillingModel>
{
  public void Configure(EntityTypeBuilder<SaleBillingModel> typeBuilder)
  {

  }
}