using GsServer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GsServer.EntityConfigurations;

public class PaymentConfiguration : IEntityTypeConfiguration<PaymentModel>
{
  public void Configure(EntityTypeBuilder<PaymentModel> typeBuilder)
  {

  }
}