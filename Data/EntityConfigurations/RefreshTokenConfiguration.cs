using GsServer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GsServer.EntityConfigurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenModel>
{
  public void Configure(EntityTypeBuilder<RefreshTokenModel> typeBuilder)
  {

  }
}