using GsServer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GsServer.EntityConfigurations;

public class SaleBillingConfiguration : IEntityTypeConfiguration<SaleBillingModel>
{
  public void Configure(EntityTypeBuilder<SaleBillingModel> typeBuilder)
  {

  }
}