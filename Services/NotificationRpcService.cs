using System.Security.Claims;
using AutoMapper;
using Grpc.Core;
using GsServer.Models;
using GsServer.Protobufs;

namespace GsServer.Services;

public class NotificationRpcService : NotificationService.NotificationServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<NotificationRpcService> _logger;
  private readonly IMapper _mapper;
  public NotificationRpcService(
      ILogger<NotificationRpcService> logger,
      DatabaseContext dbContext,
      IMapper mapper
    )
  {
    _logger = logger;
    _dbContext = dbContext;
    _mapper = mapper;
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

    IQueryable<GetNotificationByIdResponse> Query = _dbContext.Notifications.Select(
      Notification => _mapper.Map<GetNotificationByIdResponse>(Notification)
    );

    List<GetNotificationByIdResponse> Notifications = [];

    /// If cursor is bigger than the size of the collection you will get the following error
    /// ArgumentOutOfRangeException "Index was out of range. Must be non-negative and less than the size of the collection"
    Notifications = await Query
      .Where(x => x.NotificationId.CompareTo(Ulid.Parse(request.Cursor)) > 0)
      .Take(20)
      .ToListAsync();

    GetPaginatedNotificationsResponse response = new();

    response.Notifications.AddRange(Notifications);
    if (Notifications.Count < 20)
    {
      /// Avoiding `ArgumentOutOfRangeException`, basically, don't fetch if null
      response.NextCursor = null;
    }
    else
    {
      /// Id of the last element of the list, same value as `Users[Users.Count - 1].Id`
      response.NextCursor = Notifications[^1].NotificationId;
    }

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

    return _mapper.Map<GetNotificationByIdResponse>(Notification);
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

    Notification Notification = _mapper.Map<Notification>(request);
    Notification.CreatedBy = Ulid.Parse(UserId);

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
