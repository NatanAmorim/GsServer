using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class SubscriptionBillingConfiguration : IEntityTypeConfiguration<SubscriptionBillingModel>
{
  public void Configure(EntityTypeBuilder<SubscriptionBillingModel> typeBuilder)
  {

  }
}