using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class BackgroundJobConfiguration : IEntityTypeConfiguration<BackgroundJobModel>
{
  public void Configure(EntityTypeBuilder<BackgroundJobModel> typeBuilder)
  {

  }
}