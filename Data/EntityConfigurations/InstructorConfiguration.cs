using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class InstructorConfiguration : IEntityTypeConfiguration<InstructorModel>
{
  public void Configure(EntityTypeBuilder<InstructorModel> typeBuilder)
  {

  }
}