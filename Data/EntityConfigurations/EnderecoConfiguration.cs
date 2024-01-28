using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class EnderecoConfiguration : IEntityTypeConfiguration<Endereco>
{
  public void Configure(EntityTypeBuilder<Endereco> typeBuilder)
  {
    typeBuilder.Property(x => x.Logadouro).HasColumnType("varchar(20)");
    typeBuilder.Property(x => x.Numero).HasColumnType("varchar(10)");
  }
}