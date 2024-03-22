using GsServer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GsServer.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<UserModel>
{
  public void Configure(EntityTypeBuilder<UserModel> typeBuilder)
  {

  }
}