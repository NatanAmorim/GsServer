using gs_server.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gs_server.EntityConfigurations;

public class NotificationConfiguration : IEntityTypeConfiguration<NotificationModel>
{
  public void Configure(EntityTypeBuilder<NotificationModel> typeBuilder)
  {

  }
}