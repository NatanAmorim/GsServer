using GsServer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GsServer.EntityConfigurations;

public class AttendanceConfiguration : IEntityTypeConfiguration<AttendanceModel>
{
  public void Configure(EntityTypeBuilder<AttendanceModel> typeBuilder)
  {

  }
}