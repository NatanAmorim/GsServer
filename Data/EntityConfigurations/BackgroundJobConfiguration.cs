using GsServer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GsServer.EntityConfigurations;

public class BackgroundJobConfiguration : IEntityTypeConfiguration<BackgroundJobModel>
{
  public void Configure(EntityTypeBuilder<BackgroundJobModel> typeBuilder)
  {

  }
}