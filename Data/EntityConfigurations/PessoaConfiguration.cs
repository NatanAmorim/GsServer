using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class PessoaConfiguration : IEntityTypeConfiguration<Pessoa>
{
  public void Configure(EntityTypeBuilder<Pessoa> typeBuilder)
  {
    typeBuilder.HasIndex(x => x.Nome).IsUnique();
    typeBuilder.Property(x => x.Nome).HasColumnType("varchar(150)");
    typeBuilder.Property(x => x.Celular).HasColumnType("varchar(16)");
    typeBuilder.Property(x => x.DataNascimento).HasColumnType("varchar(10)");
    typeBuilder.Property(x => x.Cpf).HasColumnType("varchar(14)");
    typeBuilder.Property(x => x.Cep).HasColumnType("varchar(15)");
  }
}