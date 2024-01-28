using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class ProfessorConfiguration : IEntityTypeConfiguration<Professor>
{
  public void Configure(EntityTypeBuilder<Professor> typeBuilder)
  {

  }
}