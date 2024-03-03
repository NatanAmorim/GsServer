using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class TeacherConfiguration : IEntityTypeConfiguration<TeacherModel>
{
  public void Configure(EntityTypeBuilder<TeacherModel> typeBuilder)
  {

  }
}