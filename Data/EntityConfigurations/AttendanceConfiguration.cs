using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class AttendanceConfiguration : IEntityTypeConfiguration<AttendanceModel>
{
  public void Configure(EntityTypeBuilder<AttendanceModel> typeBuilder)
  {

  }
}