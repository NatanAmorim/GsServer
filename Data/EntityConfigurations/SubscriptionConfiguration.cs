using GsServer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GsServer.EntityConfigurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<SubscriptionModel>
{
  public void Configure(EntityTypeBuilder<SubscriptionModel> typeBuilder)
  {

  }
}