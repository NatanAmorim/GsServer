using GsServer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GsServer.EntityConfigurations;

public class OrderConfiguration : IEntityTypeConfiguration<OrderModel>
{
  public void Configure(EntityTypeBuilder<OrderModel> typeBuilder)
  {

  }
}