using GsServer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GsServer.EntityConfigurations;

public class InstructorConfiguration : IEntityTypeConfiguration<InstructorModel>
{
  public void Configure(EntityTypeBuilder<InstructorModel> typeBuilder)
  {

  }
}