using GsServer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GsServer.EntityConfigurations;

public class SubscriptionBillingConfiguration : IEntityTypeConfiguration<SubscriptionBillingModel>
{
  public void Configure(EntityTypeBuilder<SubscriptionBillingModel> typeBuilder)
  {

  }
}