using Grpc.Core;
using GsServer.Protobufs;

namespace GsServer.Services;

public class AttendanceRpcService : AttendanceService.AttendanceServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<AttendanceRpcService> _logger;
  public AttendanceRpcService(ILogger<AttendanceRpcService> logger, DatabaseContext dbContext)
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override Task<GetPaginatedAttendancesResponse> GetPaginated(GetPaginatedAttendancesRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<GetAttendanceByIdResponse> GetById(GetAttendanceByIdRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<CreateAttendanceResponse> Post(CreateAttendanceRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<UpdateAttendanceResponse> Put(UpdateAttendanceRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<DeleteAttendanceResponse> Delete(DeleteAttendanceRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }
}
