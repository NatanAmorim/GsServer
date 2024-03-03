using Grpc.Core;
using gs_server.Protobufs;

namespace gs_server.Services;

public class TeacherRpcService : TeacherService.TeacherServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<TeacherRpcService> _logger;
  public TeacherRpcService(ILogger<TeacherRpcService> logger, DatabaseContext dbContext)
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override Task<GetAllTeachersResponse> GetAll(GetAllTeachersRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<GetTeacherByIdResponse> GetById(GetTeacherByIdRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<GetAllTeachersOptionsResponse> GetAllOptions(GetAllTeachersOptionsRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<CreateTeacherResponse> Post(CreateTeacherRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<UpdateTeacherResponse> Put(UpdateTeacherRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<DeleteTeacherResponse> Delete(DeleteTeacherRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }
}
