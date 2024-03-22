using GsServer.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GsServer.EntityConfigurations;

public class NotificationConfiguration : IEntityTypeConfiguration<NotificationModel>
{
  public void Configure(EntityTypeBuilder<NotificationModel> typeBuilder)
  {

  }
}