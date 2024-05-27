using System.Security.Claims;
using Grpc.Core;
using GsServer.Models;
using GsServer.Protobufs;
using Microsoft.AspNetCore.Authorization;

namespace GsServer.Services;

[Authorize]
public class NotificationRpcService : NotificationService.NotificationServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<NotificationRpcService> _logger;
  public NotificationRpcService(
      ILogger<NotificationRpcService> logger,
      DatabaseContext dbContext
    )
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override async Task<GetPaginatedNotificationsResponse> GetPaginatedAsync(GetPaginatedNotificationsRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing multiple records ({RecordType}) with cursor {Cursor}",
      RequestTracerId,
      UserId,
      typeof(Notification).Name,
      request.Cursor
    );

    IQueryable<GetNotificationByIdResponse> Query;

    if (request.Cursor is null)
    {
      Query = _dbContext.Notifications
        .Select(Notification => Notification.ToGetById());
    }
    else
    {
      Query = _dbContext.Notifications
        .Where(x => x.NotificationId.CompareTo(Ulid.Parse(request.Cursor)) > 0)
        .Select(Notification => Notification.ToGetById());
    }

    List<GetNotificationByIdResponse> Notifications = await Query
      .Take(20)
      .ToListAsync();

    GetPaginatedNotificationsResponse response = new();

    response.Notifications.AddRange(Notifications);
    response.NextCursor = Notifications.LastOrDefault()?.NotificationId;

    _logger.LogInformation(
      "({TraceIdentifier}) multiple records ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(Notification).Name
    );

    return response;
  }

  public override async Task<GetNotificationByIdResponse> GetByIdAsync(GetNotificationByIdRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Notification).Name,
      request.NotificationId
    );

    Notification? Notification = await _dbContext.Notifications.FindAsync(request.NotificationId);

    if (Notification is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) record ({RecordType}) not found",
        RequestTracerId,
        typeof(Notification).Name
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Nenhum produto com ID {request.NotificationId}"
      ));
    }

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(Notification).Name
    );

    return Notification.ToGetById();
  }

  public override async Task<CreateNotificationResponse> PostAsync(CreateNotificationRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} creating new record ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(Notification).Name
    );

    Notification Notification = Notification.FromProtoRequest(request, Ulid.Parse(UserId));

    await _dbContext.AddAsync(Notification);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) created successfully, RecordId {RecordId}",
      RequestTracerId,
      typeof(Notification).Name,
      Notification.NotificationId
    );

    return new CreateNotificationResponse();
  }

  public override Task<UpdateNotificationResponse> PutAsync(UpdateNotificationRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} updating record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Notification).Name,
      request.NotificationId
    );

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) updated successfully",
      RequestTracerId,
      typeof(Notification).Name
    );

    throw new NotImplementedException();

    // TODO
    // NotificationModel? Notification = await _dbContext.Notifications.FirstOrDefaultAsync(x => x.Id == request.Id);
    // if (Notification is null)
    // {
    //   throw new RpcException(new Status(
    //     StatusCode.NotFound, $"registro não encontrado"
    //   ));
    // }

    // Notification.Name = request.Name;
    // // TODO Add Another fields

    // await _dbContext.SaveChangesAsync();
    // // TODO Log => Record (record type) ID Y was updated. Old value of (field name): (old value). New value: (new value). (This logs specific changes made to a field within a record)
    // return new UpdateNotificationResponse();
  }

  public override async Task<DeleteNotificationResponse> DeleteAsync(DeleteNotificationRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
        "({TraceIdentifier}) User {UserID} deleting record ({RecordType}) with ID ({RecordId})",
        RequestTracerId,
        UserId,
        typeof(Notification).Name,
        request.NotificationId
      );

    Notification? Notification = await _dbContext.Notifications.FindAsync(request.NotificationId);

    if (Notification is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) Error deleting record ({RecordType}) with ID {Id}, record not found",
        RequestTracerId,
        typeof(Notification).Name,
        request.NotificationId
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Erro ao remover registro, nenhum registro com ID {request.NotificationId}"
      ));
    }

    /// TODO check if record is being used before deleting it use something like PK or FK

    _dbContext.Notifications.Remove(Notification);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
          "({TraceIdentifier}) record ({RecordType}) deleted successfully",
          RequestTracerId,
          typeof(Notification).Name
        );

    return new DeleteNotificationResponse();
  }
}
