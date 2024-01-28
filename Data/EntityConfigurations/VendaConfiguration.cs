using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class VendaConfiguration : IEntityTypeConfiguration<Venda>
{
  public void Configure(EntityTypeBuilder<Venda> typeBuilder)
  {
    typeBuilder.Property(x => x.Descricao).HasColumnType("varchar(80)");

  }
}