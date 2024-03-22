using GsServer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GsServer.EntityConfigurations;

public class PromotionConfiguration : IEntityTypeConfiguration<PromotionModel>
{
  public void Configure(EntityTypeBuilder<PromotionModel> typeBuilder)
  {

  }
}