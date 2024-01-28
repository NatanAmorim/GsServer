using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
  public void Configure(EntityTypeBuilder<Cliente> typeBuilder)
  {

  }
}