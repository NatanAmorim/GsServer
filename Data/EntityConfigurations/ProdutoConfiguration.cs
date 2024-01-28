using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
{
  public void Configure(EntityTypeBuilder<Produto> typeBuilder)
  {

  }
}