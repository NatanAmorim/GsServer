using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<SubscriptionModel>
{
  public void Configure(EntityTypeBuilder<SubscriptionModel> typeBuilder)
  {

  }
}