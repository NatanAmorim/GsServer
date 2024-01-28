using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class AulaConfiguration : IEntityTypeConfiguration<Aula>
{
  public void Configure(EntityTypeBuilder<Aula> typeBuilder)
  {
    typeBuilder.HasIndex(x => x.Nome).IsUnique();
    typeBuilder.Property(x => x.Nome).HasColumnType("varchar(20)");
  }
}