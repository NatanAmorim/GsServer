using System.Security.Claims;
using Grpc.Core;
using GsServer.Models;
using GsServer.Protobufs;
using Microsoft.AspNetCore.Authorization;

namespace GsServer.Services;

[Authorize]
public class AttendanceRpcService : AttendanceService.AttendanceServiceBase
{
  private readonly ILogger<AttendanceRpcService> _logger;
  private readonly DatabaseContext _dbContext;
  public AttendanceRpcService(
      ILogger<AttendanceRpcService> logger,
      DatabaseContext dbContext
    )
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override async Task<GetPaginatedAttendancesResponse> GetPaginatedAsync(GetPaginatedAttendancesRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing multiple records ({RecordType}) with cursor {Cursor}",
      RequestTracerId,
      UserId,
      typeof(Attendance).Name,
      request.Cursor
    );

    IQueryable<GetAttendanceByIdResponse> Query = _dbContext.Attendances.Select(
      Attendance => Attendance.ToGetById()
    );

    List<GetAttendanceByIdResponse> Attendances = [];

    if (request.Cursor is null)
    {
      Attendances = await Query
       .Take(20)
       .ToListAsync();
    }
    else
    {
      /// If cursor is bigger than the size of the collection you will get the following error
      /// ArgumentOutOfRangeException "Index was out of range. Must be non-negative and less than the size of the collection"
      Attendances = await Query
        .Where(x => x.AttendanceId.CompareTo(Ulid.Parse(request.Cursor)) > 0)
        .Take(20)
        .ToListAsync();
    }

    GetPaginatedAttendancesResponse response = new();

    response.Attendances.AddRange(Attendances);
    if (Attendances.Count < 20)
    {
      /// Avoiding `ArgumentOutOfRangeException`, basically, don't fetch if null
      response.NextCursor = null;
    }
    else
    {
      /// Id of the last element of the list, same value as `Users[Users.Count - 1].Id`
      response.NextCursor = Attendances[^1].AttendanceId;
    }

    _logger.LogInformation(
      "({TraceIdentifier}) multiple records ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(Attendance).Name
    );
    return response;
  }

  public override async Task<GetAttendanceByIdResponse> GetByIdAsync(GetAttendanceByIdRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} accessing record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Attendance).Name,
      request.AttendanceId
    );

    Attendance? Attendance = await _dbContext.Attendances.FindAsync(request.AttendanceId);

    if (Attendance is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) record ({RecordType}) not found",
        RequestTracerId,
        typeof(Attendance).Name
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Nenhum produto com ID {request.AttendanceId}"
      ));
    }

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) accessed successfully",
      RequestTracerId,
      typeof(Attendance).Name
    );

    return Attendance.ToGetById();
  }

  public override async Task<CreateAttendanceResponse> PostAsync(CreateAttendanceRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} creating new record ({RecordType})",
      RequestTracerId,
      UserId,
      typeof(Attendance).Name
    );

    Attendance Attendance = Attendance.FromProtoRequest(request, Ulid.Parse(UserId));

    await _dbContext.AddAsync(Attendance);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) created successfully, RecordId {RecordId}",
      RequestTracerId,
      typeof(Attendance).Name,
      Attendance.AttendanceId
    );

    return new CreateAttendanceResponse();
  }

  public override Task<UpdateAttendanceResponse> PutAsync(UpdateAttendanceRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} updating record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Attendance).Name,
      request.AttendanceId
    );

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) updated successfully",
      RequestTracerId,
      typeof(Attendance).Name
    );

    throw new NotImplementedException();

    // TODO
    // AttendanceModel? Attendance = await _dbContext.Attendances.FirstOrDefaultAsync(x => x.Id == request.Id);
    // if (Attendance is null)
    // {
    //   throw new RpcException(new Status(
    //     StatusCode.NotFound, $"registro não encontrado"
    //   ));
    // }

    // Attendance.Name = request.Name;
    // // TODO Add Another fields

    // await _dbContext.SaveChangesAsync();
    // // TODO Log => Record (record type) ID Y was updated. Old value of (field name): (old value). New value: (new value). (This logs specific changes made to a field within a record)
    // return new UpdateAttendanceResponse();
  }

  public override async Task<DeleteAttendanceResponse> DeleteAsync(DeleteAttendanceRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    string UserId = context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserID} deleting record ({RecordType}) with ID ({RecordId})",
      RequestTracerId,
      UserId,
      typeof(Attendance).Name,
      request.AttendanceId
    );

    Attendance? Attendance = await _dbContext.Attendances.FindAsync(request.AttendanceId);

    if (Attendance is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) Error deleting record ({RecordType}) with ID {Id}, record not found",
        RequestTracerId,
        typeof(Attendance).Name,
        request.AttendanceId
      );
      throw new RpcException(new Status(
        StatusCode.NotFound, $"Erro ao remover registro, nenhum registro com ID {request.AttendanceId}"
      ));
    }

    /// TODO check if record is being used before deleting it use something like PK or FK

    _dbContext.Attendances.Remove(Attendance);
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "({TraceIdentifier}) record ({RecordType}) deleted successfully",
      RequestTracerId,
      typeof(Attendance).Name
    );

    return new DeleteAttendanceResponse();
  }
}
