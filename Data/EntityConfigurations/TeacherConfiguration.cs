using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
  public void Configure(EntityTypeBuilder<Teacher> typeBuilder)
  {

  }
}