using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<UserModel>
{
  public void Configure(EntityTypeBuilder<UserModel> typeBuilder)
  {

  }
}