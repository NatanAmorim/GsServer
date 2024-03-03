using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenModel>
{
  public void Configure(EntityTypeBuilder<RefreshTokenModel> typeBuilder)
  {

  }
}