using GsServer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GsServer.EntityConfigurations;

public class DisciplineConfiguration : IEntityTypeConfiguration<DisciplineModel>
{
  public void Configure(EntityTypeBuilder<DisciplineModel> typeBuilder)
  {
    typeBuilder.HasIndex(x => x.Name).IsUnique();
    typeBuilder.Property(x => x.Name).HasColumnType("varchar(20)");
  }
}