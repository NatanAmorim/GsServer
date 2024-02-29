using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
  public void Configure(EntityTypeBuilder<Attendance> typeBuilder)
  {

  }
}