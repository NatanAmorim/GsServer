using Grpc.Core;
using GsServer.Protobufs;

namespace GsServer.Services;

public class DisciplineRpcService : DisciplineService.DisciplineServiceBase
{
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<DisciplineRpcService> _logger;
  public DisciplineRpcService(ILogger<DisciplineRpcService> logger, DatabaseContext dbContext)
  {
    _logger = logger;
    _dbContext = dbContext;
  }

  public override Task<GetPaginatedDisciplinesResponse> GetPaginated(GetPaginatedDisciplinesRequest request, ServerCallContext context)
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
