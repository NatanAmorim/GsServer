using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class PaymentConfiguration : IEntityTypeConfiguration<PaymentModel>
{
  public void Configure(EntityTypeBuilder<PaymentModel> typeBuilder)
  {

  }
}