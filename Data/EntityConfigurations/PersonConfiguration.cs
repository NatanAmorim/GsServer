using GsServer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GsServer.EntityConfigurations;

public class PersonConfiguration : IEntityTypeConfiguration<PersonModel>
{
  public void Configure(EntityTypeBuilder<PersonModel> typeBuilder)
  {
    typeBuilder.HasIndex(x => x.Name).IsUnique();
    typeBuilder.Property(x => x.Name).HasColumnType("varchar(150)");
    typeBuilder.Property(x => x.MobilePhoneNumber).HasColumnType("varchar(16)");
    typeBuilder.Property(x => x.BirthDate).HasColumnType("varchar(10)");
    typeBuilder.Property(x => x.Cpf).HasColumnType("varchar(14)");
  }
}