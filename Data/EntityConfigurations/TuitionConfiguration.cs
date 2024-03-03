using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class TuitionConfiguration : IEntityTypeConfiguration<TuitionModel>
{
  public void Configure(EntityTypeBuilder<TuitionModel> typeBuilder)
  {

  }
}