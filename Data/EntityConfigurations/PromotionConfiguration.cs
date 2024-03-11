using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class PromotionConfiguration : IEntityTypeConfiguration<PromotionModel>
{
  public void Configure(EntityTypeBuilder<PromotionModel> typeBuilder)
  {

  }
}