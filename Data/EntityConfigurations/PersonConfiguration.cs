using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
  public void Configure(EntityTypeBuilder<Person> typeBuilder)
  {
    typeBuilder.HasIndex(x => x.Name).IsUnique();
    typeBuilder.Property(x => x.Name).HasColumnType("varchar(150)");
    typeBuilder.Property(x => x.MobilePhone).HasColumnType("varchar(16)");
    typeBuilder.Property(x => x.BirthDate).HasColumnType("varchar(10)");
    typeBuilder.Property(x => x.Cpf).HasColumnType("varchar(14)");
  }
}