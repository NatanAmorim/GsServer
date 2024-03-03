using Grpc.Core;
using gs_server.Protobufs;

namespace gs_server.Services;

public class DisciplineRpcService : DisciplineService.DisciplineServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<DisciplineRpcService> _logger;
  public DisciplineRpcService(ILogger<DisciplineRpcService> logger, DatabaseContext dbContext)
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override Task<GetAllDisciplinesResponse> GetAll(GetAllDisciplinesRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<GetDisciplineByIdResponse> GetById(GetDisciplineByIdRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<CreateDisciplineResponse> Post(CreateDisciplineRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<UpdateDisciplineResponse> Put(UpdateDisciplineRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override Task<DeleteDisciplineResponse> Delete(DeleteDisciplineRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }
}
