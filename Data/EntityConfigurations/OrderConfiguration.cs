using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
  public void Configure(EntityTypeBuilder<Order> typeBuilder)
  {

  }
}